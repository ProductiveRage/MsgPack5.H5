using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestNullStringList : SuccessTestItem<List<string>>
    {
        public TestNullStringList() : base(null) { }
    }
}