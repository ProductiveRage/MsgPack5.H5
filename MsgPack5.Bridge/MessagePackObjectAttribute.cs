﻿using System;

namespace MsgPack5.Bridge
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class MessagePackObjectAttribute : Attribute { }
}