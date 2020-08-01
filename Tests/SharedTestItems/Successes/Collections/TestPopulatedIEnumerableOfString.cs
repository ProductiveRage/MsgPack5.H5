using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestPopulatedIEnumerableOfString : SuccessTestItem<IEnumerable<string>>
    {
        public TestPopulatedIEnumerableOfString() : base(new[] { "abc", "def" }) { }
    }
}