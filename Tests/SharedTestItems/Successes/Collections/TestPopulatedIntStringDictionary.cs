using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestPopulatedKeyStringDictionary : SuccessTestItem<Dictionary<int, string>>
    {
        public TestPopulatedKeyStringDictionary() : base(new Dictionary<int, string> { { 1, "One" }, { 2, "Two" } }) { }
    }
}