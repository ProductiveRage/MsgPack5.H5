using System;
using System.Collections.Generic;

namespace MsgPack5.Bridge
{
    public sealed class MsgPack5Decoder
    {
        public delegate object Decoder(IBuffer buffer);

        private readonly Func<sbyte, Decoder> _customDecoderLookup;
        public MsgPack5Decoder(Func<sbyte, Decoder> customDecoderLookup = null) => _customDecoderLookup = customDecoderLookup;

        public object Decode(byte[] data) => Decode(new DefaultBuffer(data ?? throw new ArgumentNullException(nameof(data))));

        public object Decode(IBuffer buf)
        {
            var result = TryDecode(buf, 0);
            if (result.NumberOfBytesConsumed == 0)
                throw new IncompleteBufferError();

            buf.Consume(result.NumberOfBytesConsumed);
            return result.Value;
        }

        private DecodeResult TryDecode(IBuffer buf, ulong initialOffset)
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

            if (first < 0x80) // 7-bits positive int
                return new DecodeResult(first, 1);

            if ((first & 0xf0) == 0x80) // we have a map with less than 15 elements
            {
                var length = (uint)(first & 0x0f);
                var headerSize = offset - initialOffset;
                return DecodeMap(buf, offset, length, headerSize);
            }

            if ((first & 0xf0) == 0x90) // we have an array with less than 15 elements
            {
                var length = (uint)(first & 0x0f);
                var headerSize = offset - initialOffset;
                return DecodeArray(buf, offset, length, headerSize);
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
                return DecodeExt(buf, offset, type, length, size);
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
                return DecodeExt(buf, offset, type, size - 2, 2);
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
                return DecodeArray(buf, offset, length, size);
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
                        return DecodeMap(buf, offset, length, 3);

                    case 0xdf:
                        length = buf.ReadUInt32BE(offset);
                        offset += 4;
                        return DecodeMap(buf, offset, length, 5);
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

        private DecodeResult DecodeArray(IBuffer buf, ulong initialOffset, ulong length, ulong headerLength)
        {
            var (result, numberOfBytesConsumed) = DecodeArrayInternal(buf, initialOffset, length, headerLength);
            return new DecodeResult(result, numberOfBytesConsumed);
        }

        private (object[] result, ulong numberOfBytesConsumed) DecodeArrayInternal(IBuffer buf, ulong initialOffset, ulong length, ulong headerLength)
        {
            var offset = initialOffset;
            var result = new object[length];
            for (ulong i = 0; i < length; i++)
            {
                var decodeResult = TryDecode(buf, offset);
                if (decodeResult.NumberOfBytesConsumed == 0)
                    return (null, 0);
                result[i] = decodeResult.Value;
                offset += decodeResult.NumberOfBytesConsumed;
            }
            return (result, headerLength + offset - initialOffset);
        }

        private DecodeResult DecodeMap(IBuffer buf, ulong offset, ulong length, ulong headerLength)
        {
            var temp = DecodeArrayInternal(buf, offset, 2 * length, headerLength);
            if (temp.numberOfBytesConsumed == 0)
                return DecodeResult.Failed;

            var result = temp.result;
            var consumedBytes = temp.numberOfBytesConsumed;
            var mapping = new Dictionary<object, object>();
            for (ulong i = 0; i < 2 * length; i += 2)
            {
                var key = result[i];
                var val = result[i + 1];
                mapping[key] = mapping[val];
            }
            return new DecodeResult(mapping, consumedBytes);
        }

        private DecodeResult DecodeExt(IBuffer buf, ulong offset, sbyte type, ulong size, ulong headerLength)
        {
            var decoder = _customDecoderLookup?.Invoke(type);
            if (decoder == null)
                throw new InvalidOperationException("Unable to find ext type " + type);

            var value = decoder(buf.SliceAsBuffer(offset, offset + size));
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