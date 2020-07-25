using System;

namespace MessagePack.Tests.SharedTestItems.Successes.PrimitiveLikes
{
    /// <summary>
    /// This is a date time that it should be possible to serialise in the second largest of three formats - it's still within a reasonable range but it has a higher precision
    /// </summary>
    internal sealed class TestDateTimeMidPrecision : SuccessTestItem<DateTime>
    {
        public TestDateTimeMidPrecision() : base(new DateTime(2020, 6, 6, 23, 57, 42, 12)) { }
    }
}