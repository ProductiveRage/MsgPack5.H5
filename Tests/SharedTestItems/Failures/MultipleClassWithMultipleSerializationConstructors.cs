using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class MultipleClassWithMultipleSerializationConstructors : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithStringAndIntProperties);
        public Type DeserialiseAs => typeof(ClassWithMultipleSerializationConstructors);
        public object Value => new ClassWithStringAndIntProperties { Key = 123, ID = "ABC" };
    }
}