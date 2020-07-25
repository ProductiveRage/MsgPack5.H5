using System;

namespace MessagePack
{
    public sealed class TypeWithMultipleSerializationConstructorsException : Exception
    {
        public TypeWithMultipleSerializationConstructorsException(Type type) : base(GetMessage(type)) => Type = type ?? throw new ArgumentNullException(nameof(type));

        public Type Type { get; }

        private static string GetMessage(Type type) => $"Types for deserialisation may have only one [SerializationConstructor] attibute at most onst, which is not the case for {type?.FullName}";
    }
}