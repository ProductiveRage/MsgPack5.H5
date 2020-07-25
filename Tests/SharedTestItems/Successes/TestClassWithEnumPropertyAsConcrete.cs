using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestClassWithEnumPropertyAsConcrete : SuccessTestItem<ClassWithEnumProperty>
    {
        public TestClassWithEnumPropertyAsConcrete() : base(new ClassWithEnumProperty { Day = DayOfWeek.Tuesday }) { }
    }
}