using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestClassWithStringArrayAndIntArrayPropertiesAsInterface : SuccessTestItem<IUnionExample>
    {
        public TestClassWithStringArrayAndIntArrayPropertiesAsInterface() : base(new ClassWithStringArrayAndIntArrayProperties { Roles = new[] { "Tester", "Cat Herder" } }) { }
    }
}