using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    /// <summary>
    /// This illustrates that when populating a target type via property setters that it's fine if there are missing values (eg. if the target type has two key'd properties but there is only data for one then that's fine, so long as that
    /// single entry is of an appropriate type)
    /// </summary>
    internal sealed class TestFromClassWithStringPropertyToClassWithStringAndIntProperties : SuccessTestItem<ClassWithIntProperty, ClassWithIntAndStringProperties>
    {
        public TestFromClassWithStringPropertyToClassWithStringAndIntProperties() : base(new ClassWithIntProperty { Key = 12 }) { }
    }
}