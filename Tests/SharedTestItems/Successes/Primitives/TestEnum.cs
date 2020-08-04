using System;

namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestEnum : SuccessTestItem<DayOfWeek>
    {
        public TestEnum() : base(DayOfWeek.Thursday) { }
    }
}