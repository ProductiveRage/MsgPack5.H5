using System;

namespace MsgPack5.H5
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