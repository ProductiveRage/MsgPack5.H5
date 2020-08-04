using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestEmptyKeyStringDictionary : SuccessTestItem<Dictionary<int, string>>
    {
        public TestEmptyKeyStringDictionary() : base(new Dictionary<int, string>()) { }
    }
}