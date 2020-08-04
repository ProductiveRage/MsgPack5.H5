namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithStringProperty // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public string Key { get; set; }
    }
}