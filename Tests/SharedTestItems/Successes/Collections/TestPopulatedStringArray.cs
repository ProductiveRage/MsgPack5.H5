namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    internal sealed class TestPopulatedStringArray : SuccessTestItem<string[]>
    {
        public TestPopulatedStringArray() : base(new[] { "abc", "def" }) { }
    }
}