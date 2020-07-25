﻿using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestClassWithEnumPropertyAsInterface : SuccessTestItem<IUnionExample>
    {
        public TestClassWithEnumPropertyAsInterface() : base(new ClassWithEnumProperty { Day = DayOfWeek.Tuesday }) { }
    }
}