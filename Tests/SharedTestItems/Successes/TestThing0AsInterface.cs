using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestThing0AsInterface : SuccessTestItem<IThing>
    {
        public TestThing0AsInterface() : base(new Thing0 { Name = "Dan", Age = 123 }) { }
    }
}