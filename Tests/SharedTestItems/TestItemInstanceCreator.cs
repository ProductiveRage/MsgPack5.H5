using System;

namespace MessagePack.Tests.SharedTestItems
{
    internal static class TestItemInstanceCreator
    {
        public static ITestItem GetInstance(string testItemTypeName)
        {
            if (string.IsNullOrWhiteSpace(testItemTypeName))
                throw new ArgumentException("null/blank/whitespace-only value - invalid", nameof(testItemTypeName));

            var testItemType = Type.GetType(testItemTypeName);
            if (testItemType is null)
                throw new ArgumentException("does not relate to an accessible type: " + testItemTypeName, nameof(testItemTypeName));

            var ctor = testItemType.GetConstructor(new Type[0]);
            if (ctor is null)
                throw new ArgumentException("does not have an accessible parameter-less constructor: " + testItemTypeName, nameof(testItemTypeName));

            try
            {
                return (ITestItem)ctor.Invoke(null);
            }
            catch (Exception e)
            {
                throw new ArgumentException("constructor failed: " + testItemTypeName, nameof(testItemTypeName), e);
            }
        }
    }
}