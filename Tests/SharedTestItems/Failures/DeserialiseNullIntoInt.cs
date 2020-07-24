using System;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class DeserialiseNullIntoInt : ITestItem
    {
        public Type SerialiseAs => typeof(object);
        public Type DeserialiseAs => typeof(int);
        public object Value => null;
    }
}