namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// This is used to illustrate that trying to deserialise to a type that has more constructor arguments than it has key'd properties will fail (when the .NET library constructs a de/serialiser in its FormatterCache, it will be unhappy
    /// if this is the case and so the deserialiation will fail - to test this, we'll have to serialise a different type but then deserialise TO this type)
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithIntAndStringViaConstructorArgumentsButOnlyOneIntProperty // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithIntAndStringViaConstructorArgumentsButOnlyOneIntProperty(int key, string _)
        {
            Key = key;
        }

        [Key(0)]
        public int Key { get; }
    }
}