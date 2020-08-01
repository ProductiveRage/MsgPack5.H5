namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithIntPropertySetByConstructor // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithIntPropertySetByConstructor(int key) => Key = key;
        
        [Key(0)]
        public int Key { get; }
    }
}