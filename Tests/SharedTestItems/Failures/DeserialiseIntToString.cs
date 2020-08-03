using System;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class DeserialiseIntToString : ITestItem
    {
        public Type SerialiseAs => typeof(int);
        public Type DeserialiseAs => typeof(string);
        public object Value => 123;
    }
}