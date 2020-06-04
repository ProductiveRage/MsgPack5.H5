using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestThing1AsConcrete : TestThing1
    {
        public override Type DeserialiseAs => typeof(Thing1);
    }
}