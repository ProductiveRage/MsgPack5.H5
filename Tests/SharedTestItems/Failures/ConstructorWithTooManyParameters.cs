using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class ConstructorWithTooManyParameters : ITestItem
    {
        // Can't use ClassWithTooManyConstructorArguments as the SerialiseAs type as the .NET library won't serialise it (because it is unhappy that the number of key'd members doesn't match the number of constructor arguments) but we want to mimic the case
        // of deserialising an older version of a type into a new version that has more members and more constructor arguments, so we need it to serialise happily but throw the appropriate exception when deserialisation is attempted
        public Type SerialiseAs => typeof(ClassWithIntProperty);
        public Type DeserialiseAs => typeof(ClassWithTooManyConstructorArguments);
        public object Value => new ClassWithIntProperty { Key = 123 };
    }
}