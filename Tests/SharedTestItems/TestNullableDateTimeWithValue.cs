using System;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestNullableDateTimeWithNoValue : ConcreteTypeTestItem<DateTime?>
    {
        public TestNullableDateTimeWithNoValue() : base(null) { }
    }
}