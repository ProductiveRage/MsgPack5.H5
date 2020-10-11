using System.Collections.Immutable;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestPopulatedStringImmutableList : SuccessTestItem<ImmutableList<string>>
    {
        public TestPopulatedStringImmutableList() : base(ImmutableList<string>.Empty.AddRange(new[] { "abc", "def" })) { }
    }
}