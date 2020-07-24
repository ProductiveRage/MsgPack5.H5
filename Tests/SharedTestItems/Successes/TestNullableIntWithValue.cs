namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestNullableIntWithValue : SuccessTestItem<int?>
    {
        public TestNullableIntWithValue() : base(42) { }
    }
}