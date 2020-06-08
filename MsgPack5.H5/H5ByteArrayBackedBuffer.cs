using System;
using static H5.Core.es5;

namespace MsgPack5.H5
{
    /// <summary>
    /// 2020-06-08 DWR: This has been made internal until it gets removed (or it needs to be kept for something, in which case it will be made public again) - TODO: Review this soon
    /// </summary>
    internal sealed class H5ByteArrayBackedBuffer : ByteArrayBackedBuffer
    {
        private readonly byte[] _data;
        public H5ByteArrayBackedBuffer(byte[] data) => _data = data ?? throw new ArgumentNullException(nameof(data));

        public override uint Length => (uint)_data.Length - _position;

        public override byte this[uint offset]
        {
            get
            {
                CheckPosition(numberOfBytesRequired: 1);
                return (byte)_data.GetValue((int)(offset + _position));
            }
        }

        public override Uint8Array Slice(uint start, uint size)
        {
            CheckPosition(numberOfBytesRequired: size);
            var slice = new byte[size];
            Array.Copy(src: _data, spos: start + _position, dst: slice, dpos: 0, len: size);
            return new Uint8Array(slice);
        }

        public override IBuffer SliceAsBuffer(uint start, uint size) => new H5ByteArrayBackedBuffer(Slice(start, size).FreeCastToByteArray());

        public override sbyte ReadInt8(uint offset)
        {
            CheckPosition(numberOfBytesRequired: 1);
            return (sbyte)_data[offset];
        }
    }
}