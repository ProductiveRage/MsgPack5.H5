using MessagePack.Tests.SharedTypes.Unions;

namespace MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions
{
    internal sealed class TestClassWithIUnionExampleArrayProperty : SuccessTestItem<ClassWithIUnionExampleArrayProperty>
    {
        public TestClassWithIUnionExampleArrayProperty()
            : base(
                new ClassWithIUnionExampleArrayProperty
                {
                    Items = new IUnionExample[]
                    {
                        new TestClassWithStringAndIntPropertiesAsConcrete().Value,
                        new TestClassWithStringArrayAndIntArrayPropertiesAsConcrete().Value,
                        new TestClassWithEnumPropertyAsConcrete().Value
                    }
                }
            ) { }
    }
}