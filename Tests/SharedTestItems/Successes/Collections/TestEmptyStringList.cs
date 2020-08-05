using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestEmptyStringList : SuccessTestItem<List<string>>
    {
        public TestEmptyStringList() : base(new List<string>()) { }
    }
}