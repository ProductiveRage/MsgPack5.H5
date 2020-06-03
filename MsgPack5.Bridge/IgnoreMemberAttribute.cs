using System;

namespace MsgPack5.Bridge
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class IgnoreMemberAttribute : Attribute { }
}