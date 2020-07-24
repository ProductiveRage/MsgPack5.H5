using System;

namespace UnitTests
{
    internal sealed class ExceptionSummary
    {
        public ExceptionSummary(Type exceptionType, string message)
        {
            ExceptionType = exceptionType ?? throw new ArgumentNullException(nameof(exceptionType));
            Message = !string.IsNullOrWhiteSpace(message) ? message : throw new ArgumentException("may not be null, blank or whitespace-only", nameof(message));
        }

        public Type ExceptionType { get; }
        public string Message { get; }
    }
}