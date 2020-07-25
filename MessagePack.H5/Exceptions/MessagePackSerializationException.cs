using System;

namespace MessagePack
{
    public sealed class MessagePackSerializationException : Exception
    {
        public MessagePackSerializationException(string message) : base(message) { }
        public MessagePackSerializationException(string message, Exception innerException) : base(message, innerException) { }
    }
}