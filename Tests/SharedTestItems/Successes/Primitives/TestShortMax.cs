namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestShortMax : SuccessTestItem<short>
    {
        public TestShortMax() : base(short.MaxValue) { }
    }
}