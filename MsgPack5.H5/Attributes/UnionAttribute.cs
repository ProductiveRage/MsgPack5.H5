using System;

namespace MessagePack
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public sealed class UnionAttribute : Attribute
    {
        public UnionAttribute(int key, Type subType)
        {
            Key = key;
            SubType = subType;
        }

        public int Key { get; }
        public Type SubType { get; }
    }
}