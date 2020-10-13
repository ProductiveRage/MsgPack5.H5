using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using H5;
using static H5.Core.es5;

namespace MessagePack
{
    public sealed class MsgPack5Decoder
    {
        public delegate object Decoder(IBuffer buffer, Type expectedType);

        public static MsgPack5Decoder Default { get; } = new MsgPack5Decoder(DateTimeDecoder.Instance); // A custom decoder may be specified but the most common case for .NET payloads feels like the standard primitives plus complex type support PLUS DateTime (which is non-standard)

        private readonly ICustomDecoder _customDecoderIfAny;
        public MsgPack5Decoder(ICustomDecoder customDecoder = null) => _customDecoderIfAny = customDecoder;

        public T Decode<T>(Uint8Array data) => Decode<T>(new Uint8ArrayBackedBuffer(data ?? throw new ArgumentNullException(nameof(data))));
        public T Decode<T>(ArrayBuffer data) => Decode<T>(new Uint8ArrayBackedBuffer(new Uint8Array(data ?? throw new ArgumentNullException(nameof(data)))));
        public T Decode<T>(byte[] data) => Decode<T>(new Uint8ArrayBackedBuffer(new Uint8Array(data ?? throw new ArgumentNullException(nameof(data)))));

        public T Decode<T>(IBuffer buf)
        {
            var result = Decode(buf, 0, typeof(T));
            buf.Consume(result.NumberOfBytesConsumed);

            // We know that Convert will return the correct type (or it will throw), so we can tell h5 that we're returning the correct type here instead of doing another unnecessary cast (particularly since the runtime casting in h5 can
            // introduce problems - this is necessary for the "ReadOnlyArray" unit tests to pass since that is a compile-time-only construct and can't be type-checked at runtime)
            return Script.Write<T>("{0}", Convert(result.Value, typeof(T)));
        }

        private object Convert(object value, Type type)
        {
            // If the value is null then there's not much we can hopefully - hopefully it's a reference type (which will be fine) or it's a value type with an operator that can handle null (if not, it's correct to fail)
            if (value is null)
            {
                // Nullable<> gets special treatment and IS allowed to be null but you couldn't write your own version of it and have an implicit (or explicit) cast operator that would translate null into an instance of a struct, that
                // isn't supported by the .NET library
                if (GetInnerTypeOfNullableIfTypeIsNullable(type) is object)
                    return null;

                if (type.IsValueType)
                    throw new MessagePackSerializationException(type);

                return null;
            }

            // If the value is directly assignable then we don't need to do anything other than cast it directly
            if (type.IsAssignableFrom(value.GetType()))
                return value;

            if (type.IsEnum)
            {
                try
                {
                    return Enum.ToObject(type, value); // Bizarrely-named method, would have through FROM-Object would make more sense! (Thanks https://stackoverflow.com/a/2660215/3813189)
                }
                catch (Exception e)
                {
                    throw new MessagePackSerializationException(type, e);
                }
            }

            // If the target type is a nullable and the value isn't null (which we know it isn't, since that was checked for at the top of this method) then get the inner type of the nullable and try to cast the value to THAT because
            // trying to cast a byte to a Nullable<int> will fail but casting a byte to an int will succeed and then we can cast THAT to Nullable<int> and all will be well
            var innerTypeIfTypeIsNullable = GetInnerTypeOfNullableIfTypeIsNullable(type);
            if (innerTypeIfTypeIsNullable is object)
                return Convert(value, innerTypeIfTypeIsNullable);

            // Primitive values like numbers and booleans get special handling as we support trying to change from one type to another - but not for ALL types, so we shouldn't try to call Convert.ChangeType when we've got an int and we
            // want a string (the call will succeed with a string representation of the value but that isn't consistent with the behaviour of the .NET library)
            if (TreatAsPrimitive(type))
            {
                try
                {
                    return System.Convert.ChangeType(value, type);
                }
                catch (Exception e)
                {
                    throw new MessagePackSerializationException(type, e);
                }
            }

            throw new MessagePackSerializationException(type);
        }

        private static bool TreatAsPrimitive(Type type) => type.IsPrimitive || (type == typeof(decimal));

        private static Type GetInnerTypeOfNullableIfTypeIsNullable(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                ? type.GetGenericArguments()[0]
                : null;
        }

        private DecodeResult Decode(IBuffer buf, uint initialOffset, Type expectedType)
        {
            if (buf.Length < initialOffset)
                throw new IncompleteBufferError();

            var bufLength = buf.Length - initialOffset;
            var offset = initialOffset;

            var first = buf.ReadUInt8(offset);
            offset += 1;

            var size = TryToGetSize(first) ?? 0;
            if (bufLength < size)
                throw new IncompleteBufferError();

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
                    throw new IncompleteBufferError();
                var result = buf.ReadUTF8String(offset, length);
                return new DecodeResult(result, length + 1);
            }
            if (inRange(0xc0, 0xc3))
                return DecodeConstants(first);
            if (inRange(0xc4, 0xc6)) // bin8/16/32
            {
                var length = buf.ReadUIntBE(offset, size - 1);
                offset += size - 1;
                if (!IsValidDataSize(length, bufLength, size))
                    throw new IncompleteBufferError();
                var result = buf.Slice(offset, length);
                return new DecodeResult(result, size + length);
            }
            if (inRange(0xc7, 0xc9)) // ext8/16/32
            {
                var length = buf.ReadUIntBE(offset, size - 2);
                offset += size - 2;
                var type = buf.ReadInt8(offset);
                offset += 1;
                if (!IsValidDataSize(length, bufLength, size))
                    throw new IncompleteBufferError();
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
                    throw new IncompleteBufferError();
                var result = buf.ReadUTF8String(offset, length);
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

        private static bool IsValidDataSize(uint dataLength, uint bufLength, uint headerLength) => bufLength >= headerLength + dataLength;

        private static DecodeResult DecodeConstants(byte first)
        {
            if (first == 0xc0) return new DecodeResult(null, 1);
            if (first == 0xc2) return new DecodeResult(false, 1);
            if (first == 0xc3) return new DecodeResult(true, 1);
            throw new InvalidOperationException("Unrecognised constant value: " + first);
        }

        private static DecodeResult DecodeSigned(IBuffer buf, uint offset, uint size)
        {
            if (size == 1)
                return new DecodeResult(buf.ReadInt8(offset), size + 1);
            if (size == 2)
                return new DecodeResult(buf.ReadInt16BE(offset), size + 1);
            if (size == 4)
                return new DecodeResult(buf.ReadInt32BE(offset), size + 1);
            if (size == 8)
                return new DecodeResult(buf.ReadInt64BE(offset), size + 1);
            throw new InvalidOperationException("Invalid size for reading signed integer: " + size);
        }

        private static DecodeResult DecodeUnsignedInt(IBuffer buf, uint offset, uint size)
        {
            var maxOffset = offset + size;
            ulong result = 0;
            while (offset < maxOffset)
            {
                result += buf.ReadUInt8(offset++) * (ulong)System.Math.Pow(256, (int)(maxOffset - offset));
            }
            return new DecodeResult(result, size + 1);
        }

        private static DecodeResult DecodeFloat(IBuffer buf, uint offset, uint size)
        {
            if (size == 4)
                return new DecodeResult(buf.ReadFloatBE(offset), size + 1);
            if (size == 8)
                return new DecodeResult(buf.ReadDoubleBE(offset), size + 1);
            throw new InvalidOperationException("Invalid size for reading floating point number: " + size);
        }

        private DecodeResult DecodeArray(IBuffer buf, uint initialOffset, uint length, uint headerLength, Type expectedType)
        {
            // This may be a [Union] attribute - if so then it will will have a key that we'll use to map back to the type via [Union] attributes on this side
            if (length > 0)
            {
                // TODO: Cache this lookup? Would there be any genuine performance boost in the h5 context vs the cost of reflection in .NET?
                var typeIndex = buf.ReadUInt8(initialOffset);
                var unionAttribute = expectedType.GetCustomAttributes(inherit: false).OfType<UnionAttribute>().FirstOrDefault(union => union.Key == typeIndex);
                if (unionAttribute is object)
                {
                    if (unionAttribute.SubType is null)
                        throw new InvalidOperationException($"Union type index {typeIndex} has null {nameof(unionAttribute.SubType)} specified");

                    initialOffset += 1; // we've accounted for the reading of the "first" byte but we need to account for the "typeIndex" read since we've used it here (if we haven't, let its value be read again after this conditional)
                    var decodeResult = Decode(buf, initialOffset, unionAttribute.SubType);
                    return new DecodeResult(
                        decodeResult.Value,
                        decodeResult.NumberOfBytesConsumed + 2 // account for the extra two bytes that we read while ascertaining what type (from the [Union] attribute) this needed to be
                    );
                }
            }

            var decoder = ArrayDataDecoderRetriever.GetFor(expectedType, length, Convert);
            try
            {
                var numberOfBytesConsumed = DecodeArrayInternal(buf, initialOffset, length, headerLength, decoder.GetExpectedTypeForIndex, decoder.SetValueAtIndex);
                return new DecodeResult(decoder.GetFinalResult(), numberOfBytesConsumed);
            }
            catch (Exception e)
            {
                throw new MessagePackSerializationException(expectedType, e);
            }
        }

        /// <summary>
        /// This will return the number of bytes consumed
        /// </summary>
        private uint DecodeArrayInternal(IBuffer buf, uint initialOffset, uint length, uint headerLength, Func<uint, Type> expectedTypeForIndex, Action<uint, object> setterForIndex)
        {
            var offset = initialOffset;
            for (uint i = 0; i < length; i++)
            {
                var expectedType = expectedTypeForIndex(i);
                if (expectedType == null)
                    throw new Exception("DecodeArrayInternal received expectedTypeForIndex delegate that returned null for index " + i);
                var decodeResult = Decode(buf, offset, expectedType);
                if (decodeResult.NumberOfBytesConsumed == 0)
                    return 0;
                setterForIndex(i, decodeResult.Value);
                offset += decodeResult.NumberOfBytesConsumed;
            }
            return headerLength + offset - initialOffset;
        }

        private DecodeResult DecodeMap(IBuffer buf, uint initialOffset, uint length, uint headerLength, Type expectedType)
        {
            // TODO: MessagePack-CSharp also handles "Custom implementations of ICollection<> or IDictionary<,> with a parameterless constructor" and "Custom implementations of ICollection or IDictionary with a parameterless constructor"
            if (!expectedType.IsGenericType)
                throw new MessagePackSerializationException(expectedType);

            Type dictionaryType;
            var genericTypeArguments = expectedType.GetGenericArguments();
            if (expectedType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                dictionaryType = expectedType;
            else if (expectedType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                dictionaryType = typeof(Dictionary<,>).MakeGenericType(genericTypeArguments);
            else
                throw new MessagePackSerializationException(expectedType);

            var keyType = genericTypeArguments[0];
            var valueType = genericTypeArguments[1];
            var dictionary = (IDictionary)Activator.CreateInstance(dictionaryType);
            var offset = initialOffset;
            for (uint i = 0; i < length; i++)
            {
                var decodeKeyResult = Decode(buf, offset, keyType);
                if (decodeKeyResult.NumberOfBytesConsumed == 0)
                    throw new IncompleteBufferError();
                offset += decodeKeyResult.NumberOfBytesConsumed;

                var decodeValueResult = Decode(buf, offset, valueType);
                if (decodeValueResult.NumberOfBytesConsumed == 0)
                    throw new IncompleteBufferError();
                offset += decodeValueResult.NumberOfBytesConsumed;

                // Even though the key and value types will be correct, they will be wrapped in a DecodeResult instance where the Value property is an object and the non-generic IDictionary.Add method will be upset if the
                // reference types are passed (ie. the object from DecodeResult) where a value type is expected (eg. an Int32 key on the dictionary)
                dictionary.Add(
                    Convert(decodeKeyResult.Value, keyType),
                    Convert(decodeValueResult.Value, valueType)
                );
            }
            return new DecodeResult(dictionary, headerLength + offset - initialOffset);
        }

        private DecodeResult DecodeExt(IBuffer buf, uint offset, sbyte typeCode, uint size, uint headerLength, Type expectedType)
        {
            var decoder = _customDecoderIfAny.TryToGetDecoder(typeCode);
            if (decoder is null)
                throw new InvalidOperationException("Unable to find ext type " + typeCode);

            var value = decoder(buf.SliceAsBuffer(offset, size), expectedType);
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

        private struct DecodeResult
        {
            public DecodeResult(object value, uint numberOfBytesConsumed)
            {
                Value = value;
                NumberOfBytesConsumed = numberOfBytesConsumed;
            }

            public object Value { get; }

            /// <summary>
            /// A value of zero indicates that no data was read (and so the decode attempt faileD)
            /// </summary>
            public uint NumberOfBytesConsumed { get; }
        }
    }
}