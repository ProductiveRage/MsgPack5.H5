using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class NoPublicConstructor : FailureTestItem<ClassWithIntAndStringProperties, ClassWithNoPublicConstructor>
    {
        public NoPublicConstructor() : base(new ClassWithIntAndStringProperties { Key = 123, ID = "ABC" }) { }
    }
}