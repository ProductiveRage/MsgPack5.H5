using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MsgPack5.Bridge.Internal;

namespace MsgPack5.Bridge
{
    public sealed class MsgPack5Decoder
    {
        public delegate object Decoder(IBuffer buffer, Type expectedType);

        public static MsgPack5Decoder Default { get; } = new MsgPack5Decoder();

        private readonly Func<sbyte, Decoder> _customDecoderLookup;
        public MsgPack5Decoder(Func<sbyte, Decoder> customDecoderLookup = null) => _customDecoderLookup = customDecoderLookup;

        public T Decode<T>(byte[] data) => Decode<T>(new DefaultBuffer(data ?? throw new ArgumentNullException(nameof(data))));

        public T Decode<T>(IBuffer buf)
        {
            var result = TryDecode(buf, 0, typeof(T));
            if (result.NumberOfBytesConsumed == 0)
                throw new IncompleteBufferError();

            var typedResult = (T)result.Value;
            buf.Consume(result.NumberOfBytesConsumed);
            return typedResult;
        }

        // TODO: Document the difference in scenarios between throwing an exception and returning DecodeResult.Failed
        private DecodeResult TryDecode(IBuffer buf, ulong initialOffset, Type expectedType)
        {
            if (buf.Length < initialOffset)
                return DecodeResult.Failed;

            var bufLength = buf.Length - initialOffset;
            var offset = initialOffset;

            var first = buf.ReadUInt8(offset);
            offset += 1;

            var size = TryToGetSize(first) ?? 0;
            if (bufLength < size)
                return DecodeResult.Failed;

            // This may be a [Union] attribute - if so then it will will have a key that we'll use to map back to the type via [Union] attributes on this side. Note that an 0x92 value might NOT be related to a [Union], it may also be
            // an array with less than 15 elements, which is handled further down
            if ((first == 0x92) && (bufLength > 1))
            {
                var typeIndex = buf.ReadUInt8(offset);

                // TODO: Cache this lookup?
                var unionAttribute = expectedType.GetCustomAttributes<UnionAttribute>().FirstOrDefault(union => union.Key == typeIndex);
                if (unionAttribute is object)
                {
                    if (unionAttribute.SubType is null)
                        throw new InvalidOperationException($"Union type index {typeIndex} has null {nameof(unionAttribute.SubType)} specified");

                    offset += 1; // we've accounted for the reading of the "first" byte but we need to account for the "typeIndex" read since we've used it here (if we haven't, let its value be read again after this conditional)
                    var decodeResult = TryDecode(buf, offset, unionAttribute.SubType);
                    return new DecodeResult(
                        decodeResult.Value,
                        decodeResult.NumberOfBytesConsumed + 2 // account for the extra two bytes that we read while ascertaining what type (from the [Union] attribute) this needed to be
                    );
                }
            }

            if (first < 0x80) // 7-bits positive int
                return new DecodeResult(first, 1);

            if ((first & 0xf0) == 0x80) // we have a map with less than 15 elements
            {
                var length = (uint)(first & 0x0f);
                var headerSize = offset - initialOffset;
                return DecodeMap(buf, offset, length, headerSize, expectedType);
            }

            if ((first & 0xf0) == 0x90) // we have an array with less than 15 elements
            {
                var length = (uint)(first & 0x0f);
                var headerSize = offset - initialOffset;
                return DecodeArray(buf, offset, length, headerSize, expectedType);
            }

            if ((first & 0xe0) == 0xa0) // fixstr up to 31 bytes
            {
                var length = (uint)(first & 0x1f);
                if (!IsValidDataSize(length, bufLength, 1))
                    return DecodeResult.Failed;
                var result = buf.ReadUTF8String(offset, offset + length);
                return new DecodeResult(result, length + 1);
            }
            if (inRange(0xc0, 0xc3))
                return DecodeConstants(first);
            if (inRange(0xc4, 0xc6)) // bin8/16/32
            {
                var length = buf.ReadUIntBE(offset, size - 1);
                offset += size - 1;
                if (!IsValidDataSize(length, bufLength, size))
                    return DecodeResult.Failed;
                var result = buf.Slice(offset, offset + length);
                return new DecodeResult(result, size + length);
            }
            if (inRange(0xc7, 0xc9)) // ext8/16/32
            {
                var length = buf.ReadUIntBE(offset, size - 2);
                offset += size - 2;
                var type = buf.ReadInt8(offset);
                offset += 1;
                if (!IsValidDataSize(length, bufLength, size))
                    return DecodeResult.Failed;
                return DecodeExt(buf, offset, type, length, size, expectedType);
            }
            if (inRange(0xca, 0xcb))
                return DecodeFloat(buf, offset, size - 1);
            if (inRange(0xcc, 0xcf))
                return DecodeUnsignedInt(buf, offset, size - 1);
            if (inRange(0xd0, 0xd3))
                return DecodeSigned(buf, offset, size - 1);
            if (inRange(0xd4, 0xd8)) // fixext1/2/4/8/16
            {
                var type = buf.ReadInt8(offset); // Signed
                offset += 1;
                return DecodeExt(buf, offset, type, size - 2, 2, expectedType);
            }
            if (inRange(0xd9, 0xdb)) // str8/16/32
            {
                var length = buf.ReadUIntBE(offset, size - 1);
                offset += size - 1;
                if (!IsValidDataSize(length, bufLength, size))
                    return DecodeResult.Failed;
                var result = buf.ReadUTF8String(offset, offset + length);
                return new DecodeResult(result, size + length);
            }
            if (inRange(0xdc, 0xdd)) // array16/32
            {
                var length = buf.ReadUIntBE(offset, size - 1);
                offset += size - 1;
                return DecodeArray(buf, offset, length, size, expectedType);
            }
            if (inRange(0xde, 0xdf)) // map16/32
            {
                uint length;
                switch (first)
                {
                    case 0xde:
                        // maps up to 2^16 elements - 2 bytes
                        length = buf.ReadUInt16BE(offset);
                        offset += 2;
                        return DecodeMap(buf, offset, length, 3, expectedType);

                    case 0xdf:
                        length = buf.ReadUInt32BE(offset);
                        offset += 4;
                        return DecodeMap(buf, offset, length, 5, expectedType);
                }
            }
            if (first >= 0xe0) // 5 bits negative ints
                return new DecodeResult(first - 0x100, 1);

            throw new InvalidOperationException("Type code not supported: " + first);

            bool inRange(uint start, uint end) => first >= start && first <= end;
        }

        private static bool IsValidDataSize(ulong dataLength, ulong bufLength, ulong headerLength) => bufLength >= headerLength + dataLength;

        private static DecodeResult DecodeConstants(byte first)
        {
            if (first == 0xc0) return new DecodeResult(null, 1);
            if (first == 0xc2) return new DecodeResult(false, 1);
            if (first == 0xc3) return new DecodeResult(true, 1);
            throw new InvalidOperationException("Unrecognised constant value: " + first);
        }

        private static DecodeResult DecodeSigned(IBuffer buf, ulong offset, ulong size)
        {
            if (size == 1)
                return new DecodeResult(buf.ReadInt8(offset), size + 1);
            if (size == 2)
                return new DecodeResult(buf.ReadInt16BE(offset), size + 1);
            if (size == 4)
                return new DecodeResult(buf.ReadInt32BE(offset), size + 1);
            if (size == 8)
                return new DecodeResult(ReadInt64BE(buf.SliceAsBuffer(offset, offset + 8), 0), size + 1);
            throw new InvalidOperationException("Invalid size for reading signed integer: " + size);
        }

        private static long ReadInt64BE(IBuffer buf, ulong offset)
        {
            var negate = (buf[offset] & 0x80) == 0x80;
            if (negate)
            {
                var carry = 1;
                for (var i = offset + 7; i >= offset; i--)
                {
                    var v = (buf[i] ^ 0xff) + carry;
                    buf[i] = (byte)(v & 0xff);
                    carry = v >> 8;
                }
            }
            var hi = buf.ReadUInt32BE(offset + 0);
            var lo = buf.ReadUInt32BE(offset + 4);
            return (hi * 4294967296 + lo) * (negate ? -1 : 1);
        }

        private static DecodeResult DecodeUnsignedInt(IBuffer buf, ulong offset, ulong size)
        {
            var maxOffset = offset + size;
            ulong result = 0;
            while (offset < maxOffset)
            {
                result += buf.ReadUInt8(offset++) * (ulong)Math.Pow(256, maxOffset - offset);
            }
            return new DecodeResult(result, size + 1);
        }

        private static DecodeResult DecodeFloat(IBuffer buf, ulong offset, ulong size)
        {
            if (size == 4)
                return new DecodeResult(buf.ReadFloatBE(offset), size + 1);
            if (size == 8)
                return new DecodeResult(buf.ReadDoubleBE(offset), size + 1);
            throw new InvalidOperationException("Invalid size for reading floating point number: " + size);
        }

        private DecodeResult DecodeArray(IBuffer buf, ulong initialOffset, ulong length, ulong headerLength, Type expectedType)
        {
            var decoder = ArrayDataDecoderRetriever.TryToGetFor(expectedType, length);
            if (decoder == null)
                throw new Exception("Unable to deserialise to type: " + expectedType);

            var numberOfBytesConsumed = DecodeArrayInternal(buf, initialOffset, length, headerLength, decoder.GetExpectedTypeForIndex, decoder.SetValueAtIndex);
            return new DecodeResult(decoder.GetFinalResult(), numberOfBytesConsumed);
        }

        /// <summary>
        /// This will return the number of bytes consumed
        /// </summary>
        private ulong DecodeArrayInternal(IBuffer buf, ulong initialOffset, ulong length, ulong headerLength, Func<ulong, Type> expectedTypeForIndex, Action<ulong, object> setterForIndex)
        {
            var offset = initialOffset;
            for (ulong i = 0; i < length; i++)
            {
                var expectedType = expectedTypeForIndex(i);
                if (expectedType == null)
                    throw new Exception("DecodeArrayInternal received expectedTypeForIndex delegate that returned null for index " + i);
                var decodeResult = TryDecode(buf, offset, expectedType);
                if (decodeResult.NumberOfBytesConsumed == 0)
                    return 0;
                setterForIndex(i, decodeResult.Value);
                offset += decodeResult.NumberOfBytesConsumed;
            }
            return headerLength + offset - initialOffset;
        }

        private DecodeResult DecodeMap(IBuffer buf, ulong initialOffset, ulong length, ulong headerLength, Type expectedType)
        {
            var dictionaryType = expectedType;
            while (!dictionaryType.IsGenericType || (dictionaryType.GetGenericTypeDefinition() != typeof(Dictionary<,>)))
            {
                dictionaryType = dictionaryType.BaseType;
                if (dictionaryType == null)
                    throw new Exception("Unable to deserialise to type: " + expectedType); // TODO: What's the difference between throwing an exception for a failure and returning DecodeResult.Failed when we run out of data?
            }

            var dictionaryTypeArgs = dictionaryType.GetGenericArguments();
            var keyType = dictionaryTypeArgs[0];
            var valueType = dictionaryTypeArgs[1];
            var dictionary = (IDictionary)Activator.CreateInstance(dictionaryType);
            var offset = initialOffset;
            for (ulong i = 0; i < length; i++)
            {
                var decodeKeyResult = TryDecode(buf, offset, keyType);
                if (decodeKeyResult.NumberOfBytesConsumed == 0)
                    return DecodeResult.Failed;
                offset += decodeKeyResult.NumberOfBytesConsumed;

                var decodeValueResult = TryDecode(buf, offset, valueType);
                if (decodeValueResult.NumberOfBytesConsumed == 0)
                    return DecodeResult.Failed;
                offset += decodeValueResult.NumberOfBytesConsumed;

                // Even though the key and value types will be correct, they will be wrapped in a DecodeResult instance where the Value property is an object and the non-generic IDictionary.Add method will be upset if the
                // reference types are passed (ie. the object from DecodeResult) where a value type is expected (eg. an Int32 key on the dictionary)
                dictionary.Add(Convert.ChangeType(decodeKeyResult.Value, keyType), Convert.ChangeType(decodeValueResult.Value, valueType));
            }
            return new DecodeResult(dictionary, headerLength + offset - initialOffset);
        }

        private DecodeResult DecodeExt(IBuffer buf, ulong offset, sbyte typeCode, ulong size, ulong headerLength, Type expectedType)
        {
            var decoder = _customDecoderLookup?.Invoke(typeCode);
            if (decoder == null)
                throw new InvalidOperationException("Unable to find ext type " + typeCode);

            var value = decoder(buf.SliceAsBuffer(offset, offset + size), expectedType);
            return new DecodeResult(value, headerLength + size);
        }

        private static uint? TryToGetSize(byte code)
        {
            switch (code)
            {
                default: return null;
                case 0xc4: return 2;
                case 0xc5: return 3;
                case 0xc6: return 5;
                case 0xc7: return 3;
                case 0xc8: return 4;
                case 0xc9: return 6;
                case 0xca: return 5;
                case 0xcb: return 9;
                case 0xcc: return 2;
                case 0xcd: return 3;
                case 0xce: return 5;
                case 0xcf: return 9;
                case 0xd0: return 2;
                case 0xd1: return 3;
                case 0xd2: return 5;
                case 0xd3: return 9;
                case 0xd4: return 3;
                case 0xd5: return 4;
                case 0xd6: return 6;
                case 0xd7: return 10;
                case 0xd8: return 18;
                case 0xd9: return 2;
                case 0xda: return 3;
                case 0xdb: return 5;
                case 0xde: return 3;
                case 0xdc: return 3;
                case 0xdd: return 5;
            }
        }

        public sealed class IncompleteBufferError : Exception { }

        private struct DecodeResult
        {
            public static DecodeResult Failed { get; } = new DecodeResult(null, 0);

            public DecodeResult(object value, ulong numberOfBytesConsumed)
            {
                Value = value;
                NumberOfBytesConsumed = numberOfBytesConsumed;
            }

            public object Value { get; }

            /// <summary>
            /// A value of zero indicates that no data was read (and so the decode attempt faileD)
            /// </summary>
            public ulong NumberOfBytesConsumed { get; }
        }
    }
}