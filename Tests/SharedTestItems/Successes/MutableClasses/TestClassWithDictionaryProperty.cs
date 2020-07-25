using System.Collections.Generic;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses
{
    internal sealed class TestClassWithDictionaryProperty : SuccessTestItem<ClassWithDictionaryProperty>
    {
        public TestClassWithDictionaryProperty() : base(new ClassWithDictionaryProperty { Info = new Dictionary<string, int> { { "One", 1 }, { "Two", 2 } } }) { }
    }
}