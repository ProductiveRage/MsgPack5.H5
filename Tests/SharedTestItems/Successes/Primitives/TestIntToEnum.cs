using System;

namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestIntToEnum : SuccessTestItem<DayOfWeek>
    {
        public TestIntToEnum() : base(DayOfWeek.Thursday) { }
    }
}