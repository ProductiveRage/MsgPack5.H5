using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing1AsInterface : SuccessTestItem<IThing>
    {
        public TestThing1AsInterface() : base(new Thing1 { Roles = new[] { "Tester", "Cat Herder" } }) { }
    }
}