using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class SimpleTest3 : ITestItem 
    {
        public Type DeserialiseAs => typeof(IThing);
        public object Value => new Thing0 { Name = "Dan", Age = 123 };
    }
}