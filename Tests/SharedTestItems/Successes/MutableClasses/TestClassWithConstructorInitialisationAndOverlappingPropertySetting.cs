using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    /// <summary>
    /// This test confirms that if a property can be set by a constructor AND then but a writable property setter than the setter will take precedence (it will be called after the constructor is called, even if the value was already
    /// 'ear-marked' for use in the constructor call)
    /// </summary>
    internal sealed class TestClassWithConstructorInitialisationAndOverlappingPropertySetting : SuccessTestItem<ClassWithConstructorThatSetsNothingAndMutableProperty>
    {
        public TestClassWithConstructorInitialisationAndOverlappingPropertySetting() : base(new ClassWithConstructorThatSetsNothingAndMutableProperty(key: 1) { Key = 123 }) { }
    }
}