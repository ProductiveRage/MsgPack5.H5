namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestNullableIntWithNoValue : SuccessTestItem<int?>
    {
        public TestNullableIntWithNoValue() : base(null) { }
    }
}