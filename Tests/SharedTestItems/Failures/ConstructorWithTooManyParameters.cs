using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    // Can't use ClassWithTooManyConstructorArguments as the SerialiseAs type as the .NET library won't serialise it (because it is unhappy that the number of key'd members doesn't match the number of constructor arguments) but we want to mimic the case
    // of deserialising an older version of a type into a new version that has more members and more constructor arguments, so we need it to serialise happily but throw the appropriate exception when deserialisation is attempted
    internal sealed class ConstructorWithTooManyParameters : FailureTestItem<ClassWithIntProperty, ClassWithTooManyConstructorArguments>
    {
        public ConstructorWithTooManyParameters() : base(new ClassWithIntProperty { Key = 123 }){ }
    }
}