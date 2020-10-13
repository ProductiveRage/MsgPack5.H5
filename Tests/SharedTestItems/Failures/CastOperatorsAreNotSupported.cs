using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    /// <summary>
    /// Note: The .NET library doesn't support cast operations but this library has the option to enable that if desired (see the ReadOnlyArray tests)
    /// </summary>
    internal sealed class CastOperatorsAreNotSupported : FailureTestItem<string, ClassWithImplicitOperatorFromString>
    {
        public CastOperatorsAreNotSupported() : base("abc") { }
    }
}