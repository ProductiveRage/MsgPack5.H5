using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestThingWrapper : SuccessTestItem<ThingWrapper>
    {
        public TestThingWrapper() : base(new ThingWrapper { Things = new IThing[] { new TestThing0AsConcrete().Value, new TestThing1AsConcrete().Value, new TestThing2AsConcrete().Value } }) { }
    }
}