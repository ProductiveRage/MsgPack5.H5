using System;
using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    /// <summary>
    /// This tests that nested casts between compatible types (such as an int to an enum) are supported
    /// </summary>
    internal sealed class TestIntStringDictionaryToEnumStringDictionary : SuccessTestItem<Dictionary<int, string>, Dictionary<DayOfWeek, string>>
    {
        public TestIntStringDictionaryToEnumStringDictionary() : base(new Dictionary<int, string> { { 1, "Monday" }, { 2, "Tuesday" } }) { }
    }
}