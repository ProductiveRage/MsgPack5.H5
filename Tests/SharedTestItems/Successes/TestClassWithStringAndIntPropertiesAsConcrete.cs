using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    internal sealed class TestClassWithStringAndIntPropertiesAsConcrete : SuccessTestItem<ClassWithStringAndIntProperties>
    {
        public TestClassWithStringAndIntPropertiesAsConcrete() : base(new ClassWithStringAndIntProperties { Name = "Dan", Age = 123 }) { }
    }
}