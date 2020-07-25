namespace MessagePack.Tests.SharedTestItems.Successes.Nullables
{
    internal sealed class TestIntWithValue : SuccessTestItem<int?>
    {
        public TestIntWithValue() : base(42) { }
    }
}