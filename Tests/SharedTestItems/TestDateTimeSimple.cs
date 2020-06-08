﻿using System;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    /// <summary>
    /// This is a date time that it should be possible to serialise in the most succinct manner (it's in a reasonable range and it has relatively low precision - no nanosecond values)
    /// </summary>
    internal sealed class TestDateTimeSimple : ConcreteTypeTestItem<DateTime>
    {
        public TestDateTimeSimple() : base(new DateTime(2020, 6, 6, 23, 57, 42)) { }
    }
}