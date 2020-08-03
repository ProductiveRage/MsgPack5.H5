using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    /// <summary>
    /// When the .NET MessagePack library encounters an implementation of an interface but there is no Union attribute on the interface that corresponds to the type then it knows that it won't be able to deserialise it again and so it
    /// writes away a null value. This test illustrates that (and illustrates the unusual case where a AlternateResultJson value is required in the TestData.cs file - in the vast majority of cases, we're testing for a successful round
    /// trip or for an exception, it is an unusual case where there is no exception but the output does not match the input)
    /// </summary>
    internal sealed class TestClassThatHasInterfaceButNoUnionEntryAsInterface : SuccessTestItem<IUnionExample>
    {
        public TestClassThatHasInterfaceButNoUnionEntryAsInterface() : base(new ClassThatHasInterfaceButNoUnionEntry { ID = 123 }) { }
    }
}