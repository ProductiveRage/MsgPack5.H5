namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestDecimalMax : SuccessTestItem<decimal>
    {
        public TestDecimalMax() : base(decimal.MaxValue) { }
    }
}