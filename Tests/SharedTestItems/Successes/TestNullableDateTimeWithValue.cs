using System;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal sealed class TestNullableDateTimeWithNoValue : SuccessTestItem<DateTime?>
    {
        public TestNullableDateTimeWithNoValue() : base(null) { }
    }
}