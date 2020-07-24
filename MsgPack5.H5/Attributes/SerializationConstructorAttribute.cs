using System;

namespace MessagePack
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class SerializationConstructorAttribute : Attribute { }
}