namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// This illustrates the fact that the .NET library is not happy if there the constructor parameter list doesn't match the key'd properties - here there are two explicit keys (0 and 2) and one implicit key (1) but there
    /// are only two constructor arguments and so it will consider a mismatch
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithNonConsecutiveKeyPropertiesSetByConstructor // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithNonConsecutiveKeyPropertiesSetByConstructor(int key, string id)
        {
            Key = key;
            ID = id;
        }

        [Key(0)]
        public int Key { get; }

        [Key(2)]
        public string ID { get; }
    }
}