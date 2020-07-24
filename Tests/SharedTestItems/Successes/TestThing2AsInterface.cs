using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing2AsInterface : SuccessTestItem<IThing>
    {
        public TestThing2AsInterface() : base(new Thing2 { Day = DayOfWeek.Tuesday }) { }
    }
}