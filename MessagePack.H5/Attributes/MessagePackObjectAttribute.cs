using System;

namespace MessagePack
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class MessagePackObjectAttribute : Attribute { }
}