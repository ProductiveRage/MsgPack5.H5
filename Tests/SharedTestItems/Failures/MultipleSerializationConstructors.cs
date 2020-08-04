using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class MultipleSerializationConstructors : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithIntAndStringProperties);
        public Type DeserialiseAs => typeof(ClassWithMultipleSerializationConstructors);
        public object Value => new ClassWithIntAndStringProperties { Key = 123, ID = "ABC" };
    }
}