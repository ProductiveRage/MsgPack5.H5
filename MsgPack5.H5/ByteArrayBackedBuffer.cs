﻿using System;
using System.Text;
using static H5.Core.es5;

namespace MsgPack5.H5
{
    public abstract class ByteArrayBackedBuffer : IBuffer
    {
        protected uint _position;
        protected ByteArrayBackedBuffer() => _position = 0;

        public abstract uint Length { get; }

        public abstract byte this[uint offset] { get; }

        public void Consume(uint numberOfBytes)
        {
            CheckPosition(numberOfBytesRequired: 1);
            _position += numberOfBytes;
        }

        public abstract Uint8Array Slice(uint start, uint size);

        public abstract IBuffer SliceAsBuffer(uint start, uint size);

        public abstract sbyte ReadInt8(uint offset);

        public byte ReadUInt8(uint offset) => (byte)ReadInt8(offset);

        public short ReadInt16BE(uint offset)
        {
            CheckPosition(numberOfBytesRequired: 2);
            return (short)((this[offset] << 8) | this[offset + 1]);
        }

        public ushort ReadUInt16BE(uint offset) => (ushort)ReadInt16BE(offset);

        public int ReadInt32BE(uint offset)
        {
            CheckPosition(numberOfBytesRequired: 4);
            return (this[offset] << 24) + (this[offset + 1] << 16) | (this[offset + 2] << 8) | this[offset + 3];
        }

        public uint ReadUInt32BE(uint offset) => (uint)ReadInt32BE(offset);

        public long ReadInt64BE(uint offset)
        {
            var bytes = Slice(offset, size: 8);
            var negate = (bytes[0] & 0x80) == 0x80;
            if (negate)
            {
                var carry = 1;
                for (uint i = 0; i < 8; i++)
                {
                    var indexIntoBytes = 7 - i;
                    var v = (bytes[indexIntoBytes] ^ 0xff) + carry;
                    bytes[indexIntoBytes] = (byte)(v & 0xff);
                    carry = v >> 8;
                }
            }
            var hi = ReadUInt32BE(offset);
            var lo = ReadUInt32BE(offset + 4);
            return (hi * 4294967296 + lo) * (negate ? -1 : 1);
        }

        public float ReadFloatBE(uint offset)
        {
            var bytes = Slice(offset, size: 4);
            if (BitConverter.IsLittleEndian)
            {
                var b0 = bytes[0];
                var b1 = bytes[1];
                bytes[0] = bytes[3];
                bytes[1] = bytes[2];
                bytes[2] = b1;
                bytes[3] = b0;
            }
            var value = BitConverter.ToSingle(bytes.FreeCastToByteArray(), 0);
            return float.Parse(((float)value).ToString()); // 2020-06-08 DWR: BitConverter.ToSingle has a bug in Bridge/H5 where it will return a number with incorrect precision but we can force it to the correct one by rendering as a string and re-parsing (which feels expensive but I give up)
        }

        public double ReadDoubleBE(uint offset)
        {
            var bytes = Slice(offset, size: 8);
            if (BitConverter.IsLittleEndian)
            {
                var b0 = bytes[0];
                var b1 = bytes[1];
                var b2 = bytes[2];
                var b3 = bytes[3];
                bytes[0] = bytes[7];
                bytes[1] = bytes[6];
                bytes[2] = bytes[5];
                bytes[3] = bytes[4];
                bytes[4] = b3;
                bytes[5] = b2;
                bytes[6] = b1;
                bytes[7] = b0;
            }
            return BitConverter.ToDouble(bytes.FreeCastToByteArray(), 0);
        }

        public uint ReadUIntBE(uint offset, uint size)
        {
            if (size == 1)
                return ReadUInt8(offset);
            if (size == 2)
                return ReadUInt16BE(offset);
            if (size == 4)
                return ReadUInt32BE(offset);
            throw new InvalidOperationException("Invalid UIntBE size (only support 1, 2, 4): " + size);
        }

        public string ReadUTF8String(uint start, uint size) => Encoding.UTF8.GetString(Slice(start, size).FreeCastToByteArray());

        protected void CheckPosition(uint numberOfBytesRequired)
        {
            if (Length < numberOfBytesRequired)
                throw new InvalidOperationException("Attempt to read past end of content");
        }
    }
}