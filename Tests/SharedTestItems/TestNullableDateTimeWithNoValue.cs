using System;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal sealed class TestNullableDateTimeWithValue : ConcreteTypeTestItem<DateTime?>
    {
        public TestNullableDateTimeWithValue() : base(new DateTime(2020, 6, 6, 23, 57, 42)) { }
    }
}