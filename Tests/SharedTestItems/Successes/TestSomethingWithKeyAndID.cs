using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestSomethingWithKeyAndID : SuccessTestItem<SomethingWithKeyAndID>
    {
        public TestSomethingWithKeyAndID() : base(new SomethingWithKeyAndID { Key = 123, ID = "Dan" }) { }
    }
}