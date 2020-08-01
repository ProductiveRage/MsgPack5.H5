using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.ImmutableClasses
{
    internal sealed class TestPopulatedListOfArray : SuccessTestItem<List<string>>
    {
        public TestPopulatedListOfArray() : base(new List<string> { "abc", "def" }) { }
    }
}