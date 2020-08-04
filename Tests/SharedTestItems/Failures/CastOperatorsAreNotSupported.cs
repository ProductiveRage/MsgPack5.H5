using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class CastOperatorsAreNotSupported : ITestItem
    {
        public Type SerialiseAs => typeof(string);
        public Type DeserialiseAs => typeof(ClassWithImplicitOperatorFromString);
        public object Value => "abc";
    }
}