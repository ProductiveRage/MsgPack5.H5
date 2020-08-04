using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    /// <summary>
    /// If deserialising to a type that is constructor-instantiated and you want to remove a property, it is advisable to not remove it entirely but rename it to make it clear that it is no longer for use (add mark it
    /// as Obsolete to trigger a compiler warning if it IS accessed) because this makes the key'd-value-to-constructor mapping much happier
    /// </summary>
    internal sealed class TestClassWithObsoletedPropertySetByConstructor : SuccessTestItem<ClassWithNonConsecutiveKeyProperties, ClassWithObsoletedPropertySetByConstructor>
    {
        public TestClassWithObsoletedPropertySetByConstructor() : base(new ClassWithNonConsecutiveKeyProperties { Key = 123, ID = "ABC" }) { }
    }
}