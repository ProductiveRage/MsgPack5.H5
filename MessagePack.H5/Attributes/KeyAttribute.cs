using System;

namespace MessagePack
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class KeyAttribute : Attribute
    {
        public KeyAttribute(uint key)
        {
            Key = key;
        }

        public uint Key { get; }
    }
}