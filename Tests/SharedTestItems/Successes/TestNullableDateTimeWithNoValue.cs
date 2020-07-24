using System;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestNullableDateTimeWithValue : SuccessTestItem<DateTime?>
    {
        public TestNullableDateTimeWithValue() : base(new DateTime(2020, 6, 6, 23, 57, 42)) { }
    }
}