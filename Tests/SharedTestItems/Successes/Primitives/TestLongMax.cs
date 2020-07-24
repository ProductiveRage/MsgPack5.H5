namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestLongMax : SuccessTestItem<long>
    {
        public TestLongMax() : base(long.MaxValue) { }
    }
}