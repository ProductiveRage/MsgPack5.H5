﻿namespace MessagePack.Tests.SharedTestItems.Successes.Primitives
{
    internal sealed class TestDecimal : SuccessTestItem<decimal>
    {
        public TestDecimal() : base(1.2m) { }
    }
}