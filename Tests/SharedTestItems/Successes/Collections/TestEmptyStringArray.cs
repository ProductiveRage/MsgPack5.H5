namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestEmptyStringArray : SuccessTestItem<string[]>
    {
        public TestEmptyStringArray() : base(new string[0]) { }
    }
}