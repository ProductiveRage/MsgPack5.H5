namespace MessagePack.Tests.SharedTestItems.Failures
{
    internal sealed class DeserialiseNullIntoInt : FailureTestItem<object, int>
    {
        public DeserialiseNullIntoInt() : base(null) { }
    }
}