using System;
using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestIntStringDictionaryToEnumStringDictionary : SuccessTestItem<Dictionary<int, string>, Dictionary<DayOfWeek, string>>
    {
        public TestIntStringDictionaryToEnumStringDictionary() : base(new Dictionary<int, string> { { 1, "Monday" }, { 2, "Tuesday" } }) { }
    }
}