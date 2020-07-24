namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestSByteMax : SuccessTestItem<sbyte>
    {
        public TestSByteMax() : base(sbyte.MaxValue) { }
    }
}