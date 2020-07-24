namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestDecimalMin : SuccessTestItem<decimal>
    {
        public TestDecimalMin() : base(decimal.MinValue) { }
    }
}