using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestThingWrapper : ITestItem
    {
        public Type DeserialiseAs => typeof(ThingWrapper);
        public object Value => new ThingWrapper { Things = new IThing[] { TestThing0.GetValue(), TestThing1.GetValue(), TestThing2.GetValue() } };
    }
}