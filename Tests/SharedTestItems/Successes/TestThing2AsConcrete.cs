using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing2AsConcrete : SuccessTestItem<Thing2>
    {
        public TestThing2AsConcrete() : base(new Thing2 { Day = DayOfWeek.Tuesday }) { }
    }
}