using System;
using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    /// <summary>
    /// TestClassThatHasInterfaceButNoUnionEntryAsInterface already checks parity with the .NET library regarding de/serialising implementations of an interface that have no Union attribute on the interface for that type when that type
    /// is the final result but this ensures that the same behaviour occurs when the type appears further in than the top level
    /// </summary>
    internal sealed class TestClassThatHasInterfaceButNoUnionEntryAsEntryInInterfaceArray : SuccessTestItem<IUnionExample[]>
    {
        public TestClassThatHasInterfaceButNoUnionEntryAsEntryInInterfaceArray() : base(
            new IUnionExample[]
            {
                new ClassWithEnumProperty { Day = DayOfWeek.Tuesday },
                new ClassThatHasInterfaceButNoUnionEntry { ID = 123 }, // This should be serialised as a null value as there is no Union attribute for it on IUnionExample and so it should also be deserialised as null
                new ClassWithStringAndIntProperties { Name = "Zeus", Age = 10 }
            })
        { }
    }
}