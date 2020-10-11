using System.Collections.Immutable;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestEmptyStringImmutableList : SuccessTestItem<ImmutableList<string>>
    {
        public TestEmptyStringImmutableList() : base(ImmutableList<string>.Empty) { }
    }
}