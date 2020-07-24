using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing1AsConcrete : SuccessTestItem<Thing1>
    {
        public TestThing1AsConcrete() : base(new Thing1 { Roles = new[] { "Tester", "Cat Herder" } }) { }
    }
}