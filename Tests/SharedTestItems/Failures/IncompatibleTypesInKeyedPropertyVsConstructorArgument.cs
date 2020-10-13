using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    // Have to serialise as ClassWithIntProperty because the .NET library will refuse to even serialise a ClassWithIntPropertySetByConstructorWithIncompatibleType instance
    internal sealed class IncompatibleTypesInKeyedPropertyVsConstructorArgument : FailureTestItem<ClassWithIntProperty, ClassWithIntPropertySetByConstructorWithIncompatibleType>
    {
        public IncompatibleTypesInKeyedPropertyVsConstructorArgument() : base(new ClassWithIntProperty { Key = 123 }) { }
    }
}