using System.Collections.Generic;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
    internal sealed class TestNullEmptyIntStringDictionary : SuccessTestItem<Dictionary<int, string>>
    {
        public TestNullEmptyIntStringDictionary() : base(null) { }
    }
}