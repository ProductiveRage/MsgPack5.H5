namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestPopulatedStringArray : SuccessTestItem<string[]>
    {
        public TestPopulatedStringArray() : base(new[] { "abc", "def" }) { }
    }
}