using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    internal sealed class TestClassWithStringAndIntPropertiesAsConcrete : SuccessTestItem<ClassWithStringAndIntProperties>
    {
        public TestClassWithStringAndIntPropertiesAsConcrete() : base(new ClassWithStringAndIntProperties { Name = "Dan", Age = 123 }) { }
    }
}