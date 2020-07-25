using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestClassWithStringAndIntPropertiesAsInterface : SuccessTestItem<IUnionExample>
    {
        public TestClassWithStringAndIntPropertiesAsInterface() : base(new ClassWithStringAndIntProperties { Name = "Dan", Age = 123 }) { }
    }
}