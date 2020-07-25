using System;
using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    internal sealed class TestClassWithEnumPropertyAsInterface : SuccessTestItem<IUnionExample>
    {
        public TestClassWithEnumPropertyAsInterface() : base(new ClassWithEnumProperty { Day = DayOfWeek.Tuesday }) { }
    }
}