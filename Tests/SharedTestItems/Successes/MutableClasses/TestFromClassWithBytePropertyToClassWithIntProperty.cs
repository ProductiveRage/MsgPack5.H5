using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    internal sealed class TestFromClassWithBytePropertyToClassWithIntProperty : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithByteProperty);
        public Type DeserialiseAs => typeof(ClassWithIntProperty);
        public object Value => new ClassWithByteProperty { Key = 12 };
    }
}