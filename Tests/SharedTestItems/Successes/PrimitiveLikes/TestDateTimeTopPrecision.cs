using System;

namespace MessagePack.Tests.SharedTestItems.Successes.PrimitiveLikes
{
    /// <summary>
    /// This is a date time that will require the largest of the three formats - it's still within a reasonable range but it has a higher precision
    /// </summary>
    internal sealed class TestDateTimeTopPrecision : SuccessTestItem<DateTime>
    {
        public TestDateTimeTopPrecision() : base(new DateTime(2700, 6, 6, 23, 57, 42, 12)) { }
    }
}