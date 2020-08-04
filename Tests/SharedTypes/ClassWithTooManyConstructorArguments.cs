namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// Trying to deserialise into this type should make the deserialiser unhappy because it can't match the key'd properties to the constructor arguments
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithTooManyConstructorArguments // Note: Must be public (not internal) to work with MessagePack
    {
        [SerializationConstructor]
        public ClassWithTooManyConstructorArguments(int key, string _)
        {
            Key = key;
        }

        [Key(0)]
        public int Key { get; set; }
    }
}