namespace MessagePack.Tests.SharedTestItems.Successes.Nullables
{
    internal sealed class TestIntWithNoValue : SuccessTestItem<int?>
    {
        public TestIntWithNoValue() : base(null) { }
    }
}