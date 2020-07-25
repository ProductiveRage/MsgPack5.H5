namespace MessagePack.Tests.SharedTypes
{
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