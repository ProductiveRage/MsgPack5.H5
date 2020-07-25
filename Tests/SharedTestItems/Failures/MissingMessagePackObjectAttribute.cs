using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class MissingMessagePackObjectAttribute : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithStringAndIntProperties);
        public Type DeserialiseAs => typeof(ClassThatIsNotMessagePackObject);
        public object Value => new ClassWithStringAndIntProperties { Key = 123, ID = "ABC" };
    }
}