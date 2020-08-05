using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    internal sealed class TestClassWithMultipleValidConstructors : SuccessTestItem<ClassWithMultipleValidConstructors>
    {
        public TestClassWithMultipleValidConstructors() : base(new ClassWithMultipleValidConstructors(123, "ABC", new[] { "DEF", "GHI" })) { }
    }
}