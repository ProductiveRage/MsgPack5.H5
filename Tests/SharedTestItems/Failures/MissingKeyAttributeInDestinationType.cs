using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class MissingKeyAttributeInDestinationType : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithIntAndStringProperties);
        public Type DeserialiseAs => typeof(ClassWithIntAndStringPropertiesWithMissingKeyAttribute);
        public object Value => new ClassWithIntAndStringProperties { Key = 123, ID = "ABC" };
    }
}