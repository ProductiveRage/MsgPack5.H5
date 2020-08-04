using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    /// <summary>
    /// While deserialising from data that only has content for one key into a type that has a constructor that requires TWO values will work (see TestClassWithIntPropertyToClassWithIntAndStringPropertiesViaConstructor), it will only work
    /// if the destination type is 'consistent', in that IT has two key'd properties that correspond to the constructor parameters. If the target type has a constructor with two arguments but only one key'd property then it will fail
    /// </summary>
    internal sealed class DeserialiseViaConstructorWhereTypeDoesNotHaveSufficientKeyedProperties : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithIntProperty);
        public Type DeserialiseAs => typeof(ClassWithIntAndStringViaConstructorArgumentsButOnlyOneIntProperty);
        public object Value => new ClassWithIntProperty { Key = 123 };
    }
}