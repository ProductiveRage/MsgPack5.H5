using System;

namespace MsgPack5.H5
{
    public sealed class IncompleteBufferError : Exception
    {
        public IncompleteBufferError() : base("The end of the content was reached before the data-reading completed") { }
    }
}