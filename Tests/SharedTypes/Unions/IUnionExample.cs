namespace MessagePack.Tests.SharedTypes.Unions
{
    // Note: ClassThatHasInterfaceButNoUnionEntry implements this interface but is intentionally not mentioned in a Union attribute here so that the behaviour around that scenario may be tested
    [Union(0, typeof(ClassWithStringAndIntProperties))]
    [Union(1, typeof(ClassWithStringArrayAndIntArrayProperties))]
    [Union(2, typeof(ClassWithEnumProperty))]
    public interface IUnionExample { } // Note: Must be public (not internal) to work with MessagePack
}