using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestPopulatedStringList : SuccessTestItem<List<string>>
    {
        public TestPopulatedStringList() : base(new List<string> { "abc", "def" }) { }
    }
}