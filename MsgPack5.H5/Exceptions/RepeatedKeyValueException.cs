using System;

namespace MsgPack5.H5
{
    /// <summary>
    /// This will be thrown if a type that is to be deserialised that is identified as a MessagePackObject has the same Key attribute index value assigned to multiple properties and/or fields
    /// </summary>
    public sealed class RepeatedKeyValueException : Exception
    {
        public RepeatedKeyValueException(Type type, uint key) : base(GetMessage(type, key))
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Key = key;
        }

        public Type Type { get; }
        public uint Key { get; }

        private static string GetMessage(Type type, uint key) => $"All fields and properties in [MessagePackObject] that have [Key] attributes must have unique key indexes, which is not the case for type {type?.Name} and key {key}";
    }
}