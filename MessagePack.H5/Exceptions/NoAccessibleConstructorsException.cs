using System;

namespace MessagePack
{
    public sealed class NoAccessibleConstructorsException : Exception
    {
        public NoAccessibleConstructorsException(Type type) : base(GetMessage(type)) => Type = type ?? throw new ArgumentNullException(nameof(type));
        public Type Type { get; }
        private static string GetMessage(Type type) => $"There must be a public constructor specified on a [MessagePackObject] type, which is not the case for {type?.FullName}";
    }
}