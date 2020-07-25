using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    internal sealed class TestClassWithStringAndIntPropertiesWithNoInterface : SuccessTestItem<ClassWithStringAndIntPropertiesWithNoInterface>
    {
        public TestClassWithStringAndIntPropertiesWithNoInterface() : base(new ClassWithStringAndIntPropertiesWithNoInterface { Key = 123, ID = "Dan" }) { }
    }
}