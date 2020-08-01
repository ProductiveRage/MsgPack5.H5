using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    internal sealed class TestClassWithIntProperty : SuccessTestItem<ClassWithIntPropertySetByConstructor>
    {
        public TestClassWithIntProperty() : base(new ClassWithIntPropertySetByConstructor(123)) { }
    }
}