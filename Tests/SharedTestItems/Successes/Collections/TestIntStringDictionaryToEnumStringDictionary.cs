using System;
using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestIntStringDictionaryToEnumStringDictionary : ITestItem
    {
        public Type SerialiseAs => typeof(Dictionary<int, string>);
        public Type DeserialiseAs => typeof(Dictionary<DayOfWeek, string>);
        public object Value => new Dictionary<int, string> { { 1, "Monday" }, { 2, "Tuesday" } };
    }
}