using System;
using SharedTypes;

namespace SharedTestItems
{
    internal sealed class SimpleTest2 : ITestItem 
    {
        public Type DeserialiseAs => typeof(Thing0);
        public object Value => new Thing0 { Name = "Dan", Age = 123 };
    }
}