using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    /// <summary>
    /// This is a partner to TestClassThatHasInterfaceButNoUnionEntryAsInterface but illustrates that de/serialisation is not a problem for this type when we're not trying to involve its implementation of IUnionExample
    /// </summary>
    internal sealed class TestClassThatHasInterfaceButNoUnionEntryAsConcrete : SuccessTestItem<ClassThatHasInterfaceButNoUnionEntry>
    {
        public TestClassThatHasInterfaceButNoUnionEntryAsConcrete() : base(new ClassThatHasInterfaceButNoUnionEntry { ID = 123 }) { }
    }
}