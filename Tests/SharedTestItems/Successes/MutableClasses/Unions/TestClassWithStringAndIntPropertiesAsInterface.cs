using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    internal sealed class TestClassWithStringAndIntPropertiesAsInterface : SuccessTestItem<IUnionExample>
    {
        public TestClassWithStringAndIntPropertiesAsInterface() : base(new ClassWithStringAndIntProperties { Name = "Dan", Age = 123 }) { }
    }
}