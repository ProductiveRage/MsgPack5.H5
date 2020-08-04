namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithIntAndStringPropertiesWithRepeatedKeys // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public int Key { get; set; }

        [Key(0)] // Intentionally has the same Key value as the other property to ensure that we fail in the correct manner
        public string ID { get; set; }
    }
}