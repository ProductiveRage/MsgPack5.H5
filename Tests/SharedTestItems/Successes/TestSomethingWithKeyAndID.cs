using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestSomethingWithKeyAndID : SuccessTestItem<SomethingWithKeyAndID>
    {
        public TestSomethingWithKeyAndID() : base(new SomethingWithKeyAndID { Key = 123, ID = "Dan" }) { }
    }
}