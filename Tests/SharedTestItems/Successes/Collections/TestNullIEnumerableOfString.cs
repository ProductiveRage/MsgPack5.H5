using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestNullIEnumerableOfString : SuccessTestItem<IEnumerable<string>>
    {
        public TestNullIEnumerableOfString() : base(null) { }
    }
}