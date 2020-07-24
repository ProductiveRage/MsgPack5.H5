using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing1AsInterface : SuccessTestItem<IThing>
    {
        public TestThing1AsInterface() : base(new Thing1 { Roles = new[] { "Tester", "Cat Herder" } }) { }
    }
}