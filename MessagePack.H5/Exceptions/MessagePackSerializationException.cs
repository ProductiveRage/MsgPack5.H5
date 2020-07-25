using System;

namespace MessagePack
{
    public sealed class MessagePackSerializationException : Exception
    {
        public MessagePackSerializationException(Type type, Exception innerException = null) : base(GetMessage(type ?? throw new ArgumentNullException(nameof(type))), innerException) { }
       private static string GetMessage(Type type) => $"Failed to deserialize {type.FullName} value.";
    }
}