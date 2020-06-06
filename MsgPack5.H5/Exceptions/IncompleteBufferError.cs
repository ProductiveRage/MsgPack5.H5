using System;

namespace MsgPack5.H5
{
    /// <summary>
    /// This will be thrown if the end of the content was reached before the it was expected - generally indicating corrupted / truncated data
    /// </summary>
    public sealed class IncompleteBufferError : Exception
    {
        public IncompleteBufferError() : base("The end of the content was reached before the data-reading completed") { }
    }
}