using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestEmptyIEnumerableOfString : SuccessTestItem<IEnumerable<string>>
    {
        public TestEmptyIEnumerableOfString() : base(new string[0]) { }
    }
}