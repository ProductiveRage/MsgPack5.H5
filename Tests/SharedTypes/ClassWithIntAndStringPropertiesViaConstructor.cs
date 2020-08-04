namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithIntAndStringPropertiesViaConstructor // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithIntAndStringPropertiesViaConstructor(int key, string id)
        {
            Key = key;
            ID = id;
        }

        [Key(0)]
        public int Key { get; }

        [Key(1)]
        public string ID { get; }
    }
}