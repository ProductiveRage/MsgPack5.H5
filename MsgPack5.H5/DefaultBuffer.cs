using System;
using System.Text;

namespace MsgPack5.H5
{
    public sealed class DefaultBuffer : IBuffer
    {
        private readonly byte[] _data;
        private ulong _position;
        public DefaultBuffer(byte[] data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _position = 0;
        }

        public ulong Length => (ulong)_data.LongLength - _position;

        public byte this[ulong offset]
        {
            get
            {
                CheckPosition(numberOfBytesRequired: 1);
                return (byte)_data.GetValue((long)(offset + _position));
            }
            set
            {
                CheckPosition(numberOfBytesRequired: 1);
                _data.SetValue(value, (long)(offset + _position));
            }
        }

        public void Consume(ulong numberOfBytes)
        {
            CheckPosition(numberOfBytesRequired: 1);
            _position += numberOfBytes;
        }

        public byte[] Slice(ulong start, ulong end)
        {
            if (end < start)
                throw new ArgumentOutOfRangeException(nameof(end), "can't be smaller than " + nameof(start));

            var length = end - start;
            CheckPosition(numberOfBytesRequired: length);
            var slice = new byte[length];
            Array.Copy(sourceArray: _data, sourceIndex: (long)(start + _position), destinationArray: slice, destinationIndex: 0, length: (long)length);
            return slice;
        }

        public IBuffer SliceAsBuffer(ulong start, ulong end) => new DefaultBuffer(Slice(start, end));

        public sbyte ReadInt8(ulong offset)
        {
            CheckPosition(numberOfBytesRequired: 1);
            return (sbyte)_data[offset];
        }

        public byte ReadUInt8(ulong offset) => (byte)ReadInt8(offset);

        public short ReadInt16BE(ulong offset)
        {
            CheckPosition(numberOfBytesRequired: 2);
            return (short)((this[offset] << 8) | this[offset + 1]);
        }

        public ushort ReadUInt16BE(ulong offset) => (ushort)ReadInt16BE(offset);

        public int ReadInt32BE(ulong offset)
        {
            CheckPosition(numberOfBytesRequired: 4);
            return (this[offset] << 24) + (this[offset + 1] << 16) | (this[offset + 2] << 8) | this[offset + 3];
        }

        public uint ReadUInt32BE(ulong offset) => (uint)ReadInt32BE(offset);

        public float ReadFloatBE(ulong offset)
        {
            var bytes = Slice(offset, offset + 4);
            if (BitConverter.IsLittleEndian)
            {
                var b0 = bytes[0];
                var b1 = bytes[1];
                bytes[0] = bytes[3];
                bytes[1] = bytes[2];
                bytes[2] = b1;
                bytes[3] = b0;
            }
            return BitConverter.ToSingle(bytes, 0);
        }

        public double ReadDoubleBE(ulong offset)
        {
            var bytes = Slice(offset, offset + 8);
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
            return BitConverter.ToDouble(bytes, 0);
        }

        public ulong ReadUIntBE(ulong offset, ulong size)
        {
            if (size == 1)
                return ReadUInt8(offset);
            if (size == 2)
                return ReadUInt16BE(offset);
            if (size == 4)
                return ReadUInt32BE(offset);
            throw new InvalidOperationException("Invalid UIntBE size (only support 1, 2, 4): " + size);
        }

        public string ReadUTF8String(ulong start, ulong end) => Encoding.UTF8.GetString(Slice(start, end));

        private void CheckPosition(ulong numberOfBytesRequired)
        {
            if (Length < numberOfBytesRequired)
                throw new InvalidOperationException("Attempt to read past end of content");
        }
    }
}