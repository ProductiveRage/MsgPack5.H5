﻿using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestThing0AsInterface : TestThing0
    {
        public override Type DeserialiseAs => typeof(IThing);
    }
}