using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestIntStringIDictionary : SuccessTestItem<IDictionary<int, string>>
    {
        public TestIntStringIDictionary() : base(new Dictionary<int, string> { { 1, "Monday" }, { 2, "Tuesday" } }) { }
    }
}