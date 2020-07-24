using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestThingWrapper : SuccessTestItem<ThingWrapper>
    {
        public TestThingWrapper() : base(new ThingWrapper { Things = new IThing[] { new TestThing0AsConcrete().Value, new TestThing1AsConcrete().Value, new TestThing2AsConcrete().Value } }) { }
    }
}