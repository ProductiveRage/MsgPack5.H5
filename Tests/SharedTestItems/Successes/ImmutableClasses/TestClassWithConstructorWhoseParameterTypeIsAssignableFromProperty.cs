using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    internal sealed class TestClassWithConstructorWhoseParameterTypeIsAssignableFromProperty : SuccessTestItem<ClassWithConstructorWhoseParameterTypeIsAssignableFromProperty>
    {
        public TestClassWithConstructorWhoseParameterTypeIsAssignableFromProperty() : base(new ClassWithConstructorWhoseParameterTypeIsAssignableFromProperty(new[] { 1, 2, 3 })) { }
    }
}