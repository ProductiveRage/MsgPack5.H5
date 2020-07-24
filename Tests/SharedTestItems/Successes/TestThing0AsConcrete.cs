using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing0AsConcrete : SuccessTestItem<Thing0>
    {
        public TestThing0AsConcrete() : base(new Thing0 { Name = "Dan", Age = 123 }) { }
    }
}