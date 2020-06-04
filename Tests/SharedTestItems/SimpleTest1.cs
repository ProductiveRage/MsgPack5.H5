using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class SimpleTest1 : ITestItem
    {
        public Type DeserialiseAs => typeof(SomethingWithKeyAndID);
        public object Value => new SomethingWithKeyAndID { Key = 123, ID = "Dan" };
    }
}