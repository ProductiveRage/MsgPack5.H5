using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class MultipleSerializationConstructors : FailureTestItem<ClassWithIntAndStringProperties, ClassWithMultipleSerializationConstructors>
    {
        public MultipleSerializationConstructors() : base(new ClassWithIntAndStringProperties { Key = 123, ID = "ABC" }) { }
    }
}