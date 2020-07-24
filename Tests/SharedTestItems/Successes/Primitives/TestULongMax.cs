namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestULongMax : SuccessTestItem<ulong>
    {
        public TestULongMax() : base(ulong.MaxValue) { }
    }
}