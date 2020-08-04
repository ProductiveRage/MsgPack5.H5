namespace MessagePack
{
    public interface ICustomDecoder
    {
        MsgPack5Decoder.Decoder TryToGetDecoder(sbyte typeCode);
    }
}