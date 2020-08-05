using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    internal sealed class TestClassWithMultipleConstructorsButOnlyShortestIsValid : SuccessTestItem<ClassWithMultipleConstructorsButOnlyShortestIsValid>
    {
        public TestClassWithMultipleConstructorsButOnlyShortestIsValid() : base(new ClassWithMultipleConstructorsButOnlyShortestIsValid(123, 456, new[] { "DEF", "GHI" })) { }
    }
}