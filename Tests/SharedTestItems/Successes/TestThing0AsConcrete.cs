using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing0AsConcrete : SuccessTestItem<Thing0>
    {
        public TestThing0AsConcrete() : base(new Thing0 { Name = "Dan", Age = 123 }) { }
    }
}