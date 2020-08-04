namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithIntAndStringPropertiesWithMissingKeyAttribute // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public int Key { get; set; }

        // Intentionally giving this property no [Key] attribute to test the handling of the failure case of attempting to deserialise to it
        public string ID { get; set; }
    }
}