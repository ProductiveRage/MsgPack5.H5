namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestUIntMax : SuccessTestItem<uint>
    {
        public TestUIntMax() : base(uint.MaxValue) { }
    }
}