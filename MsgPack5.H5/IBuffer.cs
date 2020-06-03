namespace MsgPack5.H5
{
    public interface IBuffer
    {
        ulong Length { get; }

        byte this[ulong offset] { get; set; }

        void Consume(ulong numberOfBytes);

        byte[] Slice(ulong start, ulong end);
        IBuffer SliceAsBuffer(ulong start, ulong end);

        sbyte ReadInt8(ulong offset);
        byte ReadUInt8(ulong offset);
        short ReadInt16BE(ulong offset);
        ushort ReadUInt16BE(ulong offset);
        int ReadInt32BE(ulong offset);
        uint ReadUInt32BE(ulong offset);
        float ReadFloatBE(ulong offset);
        double ReadDoubleBE(ulong offset);

        ulong ReadUIntBE(ulong offset, ulong size);

        string ReadUTF8String(ulong start, ulong end);
    }
}