namespace MsgPack5.H5
{
    public interface IBuffer
    {
        uint Length { get; }

        byte this[uint offset] { get; set; }

        void Consume(uint numberOfBytes);

        byte[] Slice(uint start, uint end);
        IBuffer SliceAsBuffer(uint start, uint end);

        sbyte ReadInt8(uint offset);
        byte ReadUInt8(uint offset);
        short ReadInt16BE(uint offset);
        ushort ReadUInt16BE(uint offset);
        int ReadInt32BE(uint offset);
        uint ReadUInt32BE(uint offset);
        float ReadFloatBE(uint offset);
        double ReadDoubleBE(uint offset);

        uint ReadUIntBE(uint offset, uint size);

        string ReadUTF8String(uint start, uint end);
    }
}