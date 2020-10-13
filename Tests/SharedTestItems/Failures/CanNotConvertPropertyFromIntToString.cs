using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class CanNotConvertPropertyFromIntToString : FailureTestItem<ClassWithByteProperty, ClassWithStringProperty>
    {
        public CanNotConvertPropertyFromIntToString() : base(new ClassWithByteProperty { Key = 12 }) { }
    }
}