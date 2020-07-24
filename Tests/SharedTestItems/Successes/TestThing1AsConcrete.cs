using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing1AsConcrete : SuccessTestItem<Thing1>
    {
        public TestThing1AsConcrete() : base(new Thing1 { Roles = new[] { "Tester", "Cat Herder" } }) { }
    }
}