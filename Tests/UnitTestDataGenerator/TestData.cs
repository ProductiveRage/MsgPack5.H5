using System.Collections.Generic;

namespace MsgPack5.H5.Tests.UnitTests
{
    // This file is generated by the UnitTestDataGenerator project - run that to ensure that this is current (includes all of the tests in the SharedTestItems project) and then run the Unit Tests project
    internal static class TestData
    {
        public static IEnumerable<(string testItemName, byte[] serialised)> GetItems()
        {
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestSomethingWithKeyAndID", new byte[] { 146, 123, 163, 68, 97, 110});
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing0AsConcrete", new byte[] { 146, 163, 68, 97, 110, 123});
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing0AsInterface", new byte[] { 146, 0, 146, 163, 68, 97, 110, 123});
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing1AsConcrete", new byte[] { 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192});
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing1AsInterface", new byte[] { 146, 1, 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192});
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing2AsConcrete", new byte[] { 145, 2});
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing2AsInterface", new byte[] { 146, 2, 145, 2});
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThingWrapper", new byte[] { 145, 147, 146, 0, 146, 163, 68, 97, 110, 123, 146, 1, 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192, 146, 2, 145, 2});
        }
    }
}
