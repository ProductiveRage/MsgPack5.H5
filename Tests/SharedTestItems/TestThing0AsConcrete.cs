using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestThing0AsConcrete : TestThing0
    {
        public override Type DeserialiseAs => typeof(Thing0);
    }
}