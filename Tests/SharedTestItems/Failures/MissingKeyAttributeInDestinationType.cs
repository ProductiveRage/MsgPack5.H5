using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class MissingKeyAttributeInDestinationType : FailureTestItem<ClassWithIntAndStringProperties, ClassWithIntAndStringPropertiesWithMissingKeyAttribute>
    {
        public MissingKeyAttributeInDestinationType() : base(new ClassWithIntAndStringProperties { Key = 123, ID = "ABC" }) { }
    }
}