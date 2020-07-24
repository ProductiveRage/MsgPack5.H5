using System;

namespace MessagePack
{
    public sealed class MessagePackSerializationException : Exception
    {
        public MessagePackSerializationException(string message) : base(message) { }
    }
}