namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// The .NET library will not be happy with this, so it should refuse to serialise it
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithIntPropertySetByConstructorWithIncompatibleType // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithIntPropertySetByConstructorWithIncompatibleType(string value) => ValueLength = value?.Length ?? -1;
        
        [Key(0)]
        public int ValueLength { get; }
    }
}