using System;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestNullableDateTimeWithNoValue : SuccessTestItem<DateTime?>
    {
        public TestNullableDateTimeWithNoValue() : base(null) { }
    }
}