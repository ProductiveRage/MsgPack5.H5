using System;

namespace MessagePack
{
    public sealed class NoCompatibleConstructorFoundException : Exception
    {
        public NoCompatibleConstructorFoundException(Type type) : base(GetMessage(type)) => Type = type ?? throw new ArgumentNullException(nameof(type));
        public Type Type { get; }
        private static string GetMessage(Type type) => $"There was no accessible constructor that could be matched to the key'd members on the type and so deserialisation is not supported for {type?.FullName}";
    }
}