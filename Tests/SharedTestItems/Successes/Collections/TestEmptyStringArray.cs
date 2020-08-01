namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    internal sealed class TestEmptyStringArray : SuccessTestItem<string[]>
    {
        public TestEmptyStringArray() : base(new string[0]) { }
    }
}