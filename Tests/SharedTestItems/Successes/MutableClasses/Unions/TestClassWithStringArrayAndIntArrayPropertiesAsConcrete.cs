using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    internal sealed class TestClassWithStringArrayAndIntArrayPropertiesAsConcrete : SuccessTestItem<ClassWithStringArrayAndIntArrayProperties>
    {
        public TestClassWithStringArrayAndIntArrayPropertiesAsConcrete() : base(new ClassWithStringArrayAndIntArrayProperties { Roles = new[] { "Tester", "Cat Herder" } }) { }
    }
}