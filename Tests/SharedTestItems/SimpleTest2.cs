using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class SimpleTest2 : ITestItem 
    {
        public Type DeserialiseAs => typeof(Thing0);
        public object Value => new Thing0 { Name = "Dan", Age = 123 };
    }
}