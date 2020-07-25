namespace MessagePack.Tests.SharedTypes.Unions
{
    [Union(0, typeof(ClassWithStringAndIntProperties))]
    [Union(1, typeof(ClassWithStringArrayAndIntArrayProperties))]
    [Union(2, typeof(ClassWithEnumProperty))]
    public interface IUnionExample { } // Note: Must be public (not internal) to work with MessagePack
}