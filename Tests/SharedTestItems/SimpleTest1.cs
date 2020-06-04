using System;
using SharedTypes;

namespace SharedTestItems
{
    internal sealed class SimpleTest1 : ITestItem
    {
        public Type DeserialiseAs => typeof(SomethingWithKeyAndID);
        public object Value => new SomethingWithKeyAndID { Key = 123, ID = "Dan" };
    }
}