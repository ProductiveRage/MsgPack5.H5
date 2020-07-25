using System;
using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    internal sealed class TestClassWithEnumPropertyAsConcrete : SuccessTestItem<ClassWithEnumProperty>
    {
        public TestClassWithEnumPropertyAsConcrete() : base(new ClassWithEnumProperty { Day = DayOfWeek.Tuesday }) { }
    }
}