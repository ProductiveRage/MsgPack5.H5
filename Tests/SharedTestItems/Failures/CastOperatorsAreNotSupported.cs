using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class CastOperatorsAreNotSupported : FailureTestItem<string, ClassWithImplicitOperatorFromString>
    {
        public CastOperatorsAreNotSupported() : base("abc") { }
    }
}