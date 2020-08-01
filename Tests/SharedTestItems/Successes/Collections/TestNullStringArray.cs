namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    internal sealed class TestNullStringArray : SuccessTestItem<string[]>
    {
        public TestNullStringArray() : base(null) { }
    }
}