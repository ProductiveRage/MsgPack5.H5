using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal abstract class TestThing0 : ITestItem
    {
        public static Thing0 GetValue() => new Thing0 { Name = "Dan", Age = 123 };

        public abstract Type DeserialiseAs { get; }
        public object Value => GetValue();
    }
}