using System;

namespace MsgPack5.H5
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class SerializationConstructorAttribute : Attribute { }
}