namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestIntMax : SuccessTestItem<int>
    {
        public TestIntMax() : base(int.MaxValue) { }
    }
}