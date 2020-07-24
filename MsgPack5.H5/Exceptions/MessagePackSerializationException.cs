using System;

namespace MsgPack5.H5
{
    public sealed class MessagePackSerializationException : Exception
    {
        public MessagePackSerializationException(string message) : base(message) { }
    }
}