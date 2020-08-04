using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    /// <summary>
    /// This illustrates that if we go from a source type that only has one key'd property and we try to deserialise into a type that is instantiated solely by constructor (no settable properties) that it will succeed,
    /// so long as the destination type is 'consistent', meaning that it has key'd properties that correspond to its constructor arguments
    /// </summary>
    internal sealed class TestClassWithIntPropertyToClassWithIntAndStringPropertiesViaConstructor : SuccessTestItem<ClassWithIntProperty, ClassWithIntAndStringPropertiesViaConstructor>
    {
        public TestClassWithIntPropertyToClassWithIntAndStringPropertiesViaConstructor() : base(new ClassWithIntProperty { Key = 123 }) { }
    }
}