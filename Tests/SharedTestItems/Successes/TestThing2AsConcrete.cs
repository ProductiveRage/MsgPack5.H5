using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing2AsConcrete : SuccessTestItem<Thing2>
    {
        public TestThing2AsConcrete() : base(new Thing2 { Day = DayOfWeek.Tuesday }) { }
    }
}