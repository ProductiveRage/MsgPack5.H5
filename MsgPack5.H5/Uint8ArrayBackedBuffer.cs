using System;
using static H5.Core.es5;

namespace MsgPack5.H5
{
    public sealed class Uint8ArrayBackedBuffer : ByteArrayBackedBuffer
    {
        private readonly Uint8Array _data;
        public Uint8ArrayBackedBuffer(Uint8Array data) => _data = data ?? throw new ArgumentNullException(nameof(data));

        public override uint Length => _data.byteLength - _position;

        public override byte this[uint offset]
        {
            get
            {
                CheckPosition(numberOfBytesRequired: 1);
                return _data[offset + _position];
            }
        }

        public override Uint8Array Slice(uint start, uint size)
        {
            CheckPosition(numberOfBytesRequired: size);
            return _data.slice(start, start + size); // Note: Slice returns an array that is a copy of the original data, it is not a view onto it and so changing values in the slice will not affect the source
        }

        public override IBuffer SliceAsBuffer(uint start, uint size) => new Uint8ArrayBackedBuffer(Slice(start, size));

        public override sbyte ReadInt8(uint offset)
        {
            CheckPosition(numberOfBytesRequired: 1);
            return (sbyte)_data[offset];
        }
    }
}