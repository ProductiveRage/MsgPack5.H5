namespace MessagePack.Tests.SharedTypes
{
    public sealed class ClassWithNoPublicConstructor // Note: Must be public (not internal) to work with MessagePack
    {
        private ClassWithNoPublicConstructor() { }
    }
}