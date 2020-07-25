using System;

namespace MessagePack.Tests.SharedTestItems.Successes.Nullables
{
    internal sealed class TestDateTimeWithValue : SuccessTestItem<DateTime?>
    {
        public TestDateTimeWithValue() : base(null) { }
    }
}