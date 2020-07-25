using static H5.Core.es5;

namespace MessagePack
{
    public interface IBuffer
    {
        uint Length { get; }

        byte this[uint offset] { get; }

        void Consume(uint numberOfBytes);

        Uint8Array Slice(uint start, uint size);
        IBuffer SliceAsBuffer(uint start, uint size);

        sbyte ReadInt8(uint offset);
        byte ReadUInt8(uint offset);
        short ReadInt16BE(uint offset);
        ushort ReadUInt16BE(uint offset);
        int ReadInt32BE(uint offset);
        uint ReadUInt32BE(uint offset);
        long ReadInt64BE(uint offset);
        ulong ReadUInt64BE(uint offset);
        float ReadFloatBE(uint offset);
        double ReadDoubleBE(uint offset);

        uint ReadUIntBE(uint offset, uint size);

        string ReadUTF8String(uint start, uint size);
    }
}