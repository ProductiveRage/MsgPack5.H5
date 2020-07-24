using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing2AsInterface : SuccessTestItem<IThing>
    {
        public TestThing2AsInterface() : base(new Thing2 { Day = DayOfWeek.Tuesday }) { }
    }
}