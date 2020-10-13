using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class MissingMessagePackObjectAttribute : FailureTestItem<ClassWithIntAndStringProperties, ClassThatIsNotMessagePackObject>
    {
        public MissingMessagePackObjectAttribute() : base(new ClassWithIntAndStringProperties { Key = 123, ID = "ABC" }) { }
    }
}