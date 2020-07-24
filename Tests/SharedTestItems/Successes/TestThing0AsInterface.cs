using System;
using MsgPack5.H5.Tests.SharedTypes;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing0AsInterface : SuccessTestItem<IThing>
    {
        public TestThing0AsInterface() : base(new Thing0 { Name = "Dan", Age = 123 }) { }
    }
}