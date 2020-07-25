using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestClassWithStringArrayAndIntArrayPropertiesAsConcrete : SuccessTestItem<ClassWithStringArrayAndIntArrayProperties>
    {
        public TestClassWithStringArrayAndIntArrayPropertiesAsConcrete() : base(new ClassWithStringArrayAndIntArrayProperties { Roles = new[] { "Tester", "Cat Herder" } }) { }
    }
}