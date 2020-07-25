using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    internal sealed class TestClassWithStringAndIntPropertiesAsInterface : SuccessTestItem<IUnionExample>
    {
        public TestClassWithStringAndIntPropertiesAsInterface() : base(new ClassWithStringAndIntProperties { Name = "Dan", Age = 123 }) { }
    }
}