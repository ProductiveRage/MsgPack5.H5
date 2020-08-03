namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithByteProperty // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public byte Key { get; set; }
    }
}