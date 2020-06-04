using System;
using SharedTypes;

namespace SharedTestItems
{
    internal sealed class SimpleTest3 : ITestItem 
    {
        public Type DeserialiseAs => typeof(IThing);
        public object Value => new Thing0 { Name = "Dan", Age = 123 };
    }
}