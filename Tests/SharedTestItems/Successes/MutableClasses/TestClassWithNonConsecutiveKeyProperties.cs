using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    /// <summary>
    /// The data being being deserialised has key'd properties with index 0 and 2 but nothing for index 1 - but that shouldn't be a problem
    /// </summary>
    internal sealed class TestClassWithNonConsecutiveKeyProperties : SuccessTestItem<ClassWithNonConsecutiveKeyProperties>
    {
        public TestClassWithNonConsecutiveKeyProperties() : base(new ClassWithNonConsecutiveKeyProperties { Key = 123, ID = "ABC" }) { }
    }
}