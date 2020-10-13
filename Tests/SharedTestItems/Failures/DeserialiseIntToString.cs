namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class DeserialiseIntToString : FailureTestItem<int, string>
    {
        public DeserialiseIntToString() : base(123) { }
    }
}