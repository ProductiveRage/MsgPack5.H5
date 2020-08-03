using System;

namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestByteToInt : ITestItem
    {
        public Type SerialiseAs => typeof(byte);
        public Type DeserialiseAs => typeof(int);
        public object Value => (byte)12;
    }
}