namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestLongMin : SuccessTestItem<long>
    {
        public TestLongMin() : base(long.MinValue) { }
    }
}