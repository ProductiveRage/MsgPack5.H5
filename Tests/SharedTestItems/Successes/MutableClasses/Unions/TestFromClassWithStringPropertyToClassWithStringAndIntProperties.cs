using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    /// <summary>
    /// This illustrates that when populating a target type via property setters that it's fine if there are additional values in the serialised data (eg. if the target type has only one key'd property but there is data for two - it fine,
    /// so long as that single entry is of an appropriate type and then the other will be ignored)
    /// </summary>
    internal sealed class TestFromClassWithStringAndIntPropertiesToClassWithStringProperty : SuccessTestItem<ClassWithIntAndStringProperties, ClassWithIntProperty>
    {
        public TestFromClassWithStringAndIntPropertiesToClassWithStringProperty() : base(new ClassWithIntAndStringProperties { Key = 12, ID = "ABC" }) { }
    }
}