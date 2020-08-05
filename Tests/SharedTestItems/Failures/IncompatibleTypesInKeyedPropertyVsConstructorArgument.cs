using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class IncompatibleTypesInKeyedPropertyVsConstructorArgument : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithIntProperty); // Have to serialise as this type because the .NET library will refuse to even serialise a ClassWithIntPropertySetByConstructorWithIncompatibleType instance
        public Type DeserialiseAs => typeof(ClassWithIntPropertySetByConstructorWithIncompatibleType);
        public object Value => new ClassWithIntProperty { Key = 123 };
    }
}