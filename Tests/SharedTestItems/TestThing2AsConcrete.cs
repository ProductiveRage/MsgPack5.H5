using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestThing2AsConcrete : TestThing2
    {
        public override Type DeserialiseAs => typeof(Thing2);
    }
}