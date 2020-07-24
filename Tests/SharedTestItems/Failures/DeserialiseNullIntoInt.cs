using System;

namespace MsgPack5.H5.Tests.SharedTestItems.Failures
{
    internal sealed class DeserialiseNullIntoInt : ITestItem
    {
        public Type SerialiseAs => typeof(object);
        public Type DeserialiseAs => typeof(int);
        public object Value => null;
    }
}