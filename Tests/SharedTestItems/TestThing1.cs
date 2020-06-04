using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal abstract class TestThing1 : ITestItem
    {
        public static Thing1 GetValue() => new Thing1 { Roles = new[] { "Tester", "Cat Herder" } };

        public abstract Type DeserialiseAs { get; }
        public object Value => GetValue();
    }
}