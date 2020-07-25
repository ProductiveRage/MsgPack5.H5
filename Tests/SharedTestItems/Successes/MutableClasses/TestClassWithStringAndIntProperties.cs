using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    internal sealed class TestClassWithStringAndIntProperties : SuccessTestItem<ClassWithStringAndIntProperties>
    {
        public TestClassWithStringAndIntProperties() : base(new ClassWithStringAndIntProperties { Key = 123, ID = "Dan" }) { }
    }
}