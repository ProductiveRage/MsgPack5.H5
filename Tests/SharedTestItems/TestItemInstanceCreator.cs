using System;

namespace SharedTestItems
{
    internal static class TestItemInstanceCreator
    {
        public static ITestItem GetInstance(string testItemTypeName)
        {
            if (string.IsNullOrWhiteSpace(testItemTypeName))
                throw new ArgumentException("null/blank/whitespace-only value - invalid", nameof(testItemTypeName));

            var testItemType = Type.GetType(testItemTypeName, throwOnError: false, ignoreCase: false);
            if (testItemType is null)
                throw new ArgumentException("does not relate to an accessible type: " + testItemTypeName, nameof(testItemTypeName));

            var ctor = testItemType.GetConstructor(Type.EmptyTypes);
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