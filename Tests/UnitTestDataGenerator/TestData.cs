using System.Collections.Generic;

namespace MsgPack5.H5.Tests.UnitTests
{
    internal static class TestData
    {
        public static IEnumerable<(string testItemName, byte[] serialised)> GetItems()
        {
            yield return ("SharedTestItems.SimpleTest1", new byte[] { 146, 123, 163, 68, 97, 110});
            yield return ("SharedTestItems.SimpleTest2", new byte[] { 146, 163, 68, 97, 110, 123});
            yield return ("SharedTestItems.SimpleTest3", new byte[] { 146, 0, 146, 163, 68, 97, 110, 123});
        }
    }
}
