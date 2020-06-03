using System;

namespace MsgPack5.H5
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class MessagePackObjectAttribute : Attribute { }
}