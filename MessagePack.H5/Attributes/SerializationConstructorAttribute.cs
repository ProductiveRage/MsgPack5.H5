using System;

namespace MessagePack
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
    public sealed class SerializationConstructorAttribute : Attribute { }
}