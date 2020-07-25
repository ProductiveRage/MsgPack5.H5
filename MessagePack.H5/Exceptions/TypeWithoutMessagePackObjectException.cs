using System;

namespace MessagePack
{
    /// <summary>
    /// This will be thrown if a type that is to be deserialised is not identified as a MessagePackObject (and is not one of the ones with special handling, such as primitives, strings, lists, etc..)
    /// </summary>
    public sealed class TypeWithoutMessagePackObjectException : Exception
    {
        public TypeWithoutMessagePackObjectException(Type type) : base(GetMessage(type)) => Type = type ?? throw new ArgumentNullException(nameof(type));
        public Type Type { get; }
        private static string GetMessage(Type type) => $"Types for deserialisation must have a [MessagePackObject] attibute on them, which is not the case for {type?.FullName}";
    }
}