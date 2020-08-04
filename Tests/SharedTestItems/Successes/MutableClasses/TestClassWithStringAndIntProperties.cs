using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    internal sealed class TestClassWithStringAndIntProperties : SuccessTestItem<ClassWithIntAndStringProperties>
    {
        public TestClassWithStringAndIntProperties() : base(new ClassWithIntAndStringProperties { Key = 123, ID = "Dan" }) { }
    }
}