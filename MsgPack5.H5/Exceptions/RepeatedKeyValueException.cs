using System;
using System.Reflection;

namespace MessagePack
{
    /// <summary>
    /// This will be thrown if a type that is to be deserialised that is identified as a MessagePackObject has the same Key attribute index value assigned to multiple properties and/or fields
    /// </summary>
    public sealed class RepeatedKeyValueException : Exception
    {
        public RepeatedKeyValueException(Type type, uint key, (MemberInfo Member1, MemberInfo Member2) firstCollision) : base(GetMessage(type, key))
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Key = key;
            FirstCollision = firstCollision;
            if ((firstCollision.Member1 is null) || (firstCollision.Member2 is null))
                throw new ArgumentException("neither of the member info references may be null", nameof(firstCollision));
        }

        public Type Type { get; }
        public uint Key { get; }

        /// <summary>
        /// This only specifies the first collision (the first two members encountered that repeat the same key index - there may be more)
        /// </summary>
        public (MemberInfo Member1, MemberInfo Member2) FirstCollision { get; }

        private static string GetMessage(Type type, uint key) => $"All fields and properties in [MessagePackObject] that have [Key] attributes must have unique key indexes, which is not the case for type {type?.Name} and key {key}";
    }
}