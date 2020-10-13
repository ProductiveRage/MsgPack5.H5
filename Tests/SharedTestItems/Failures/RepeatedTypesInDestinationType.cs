using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class RepeatedTypesInDestinationType : FailureTestItem<ClassWithIntAndStringProperties, ClassWithIntAndStringPropertiesWithRepeatedKeys>
    {
        public RepeatedTypesInDestinationType() : base(new ClassWithIntAndStringProperties { Key = 123, ID = "ABC" }) { }
    }
}