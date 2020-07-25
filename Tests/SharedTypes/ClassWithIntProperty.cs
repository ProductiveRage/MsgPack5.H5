namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithIntProperty // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public int Key { get; set; }
    }
}