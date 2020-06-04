using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal abstract class TestThing2 : ITestItem
    {
        public static Thing2 GetValue() => new Thing2 { Day = DayOfWeek.Tuesday };

        public abstract Type DeserialiseAs { get; }
        public object Value => GetValue();
    }
}