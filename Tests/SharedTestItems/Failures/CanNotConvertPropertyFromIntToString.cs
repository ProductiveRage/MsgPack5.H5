using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class CanNotConvertPropertyFromIntToString : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithByteProperty);
        public Type DeserialiseAs => typeof(ClassWithStringProperty);
        public object Value => new ClassWithByteProperty { Key = 12 };
    }
}