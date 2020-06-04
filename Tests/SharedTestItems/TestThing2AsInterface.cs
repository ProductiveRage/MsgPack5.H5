using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestThing2AsInterface : TestThing2
    {
        public override Type DeserialiseAs => typeof(IThing);
    }
}