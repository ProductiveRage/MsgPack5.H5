using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    internal sealed class TestFromClassWithBytePropertyToClassWithIntProperty : SuccessTestItem<ClassWithByteProperty, ClassWithIntProperty>
    {
        public TestFromClassWithBytePropertyToClassWithIntProperty() : base(new ClassWithByteProperty { Key = 12 }) { }
    }
}