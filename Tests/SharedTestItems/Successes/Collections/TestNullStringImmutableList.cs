using System.Collections.Immutable;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestNullStringImmutableList : SuccessTestItem<ImmutableList<string>>
    {
        public TestNullStringImmutableList() : base(null) { }
    }
}