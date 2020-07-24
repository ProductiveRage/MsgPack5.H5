namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestByteMax : SuccessTestItem<byte>
    {
        public TestByteMax() : base(byte.MaxValue) { }
    }
}