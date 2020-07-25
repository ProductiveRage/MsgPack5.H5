using System;

namespace MessagePack.Tests.SharedTestItems.Successes.Nullables
{
    internal sealed class TestDateTimeWithNoValue : SuccessTestItem<DateTime?>
    {
        public TestDateTimeWithNoValue() : base(new DateTime(2020, 6, 6, 23, 57, 42)) { }
    }
}