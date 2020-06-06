using System.Collections.Generic;

namespace UnitTests
{
    // This file is generated by the UnitTestDataGenerator project - run that to ensure that this is current (includes all of the tests in the SharedTestItems project) and then run the Unit Tests project
    internal static class TestData
    {
        public static IEnumerable<(string testItemName, byte[] serialised)> GetItems()
        {
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestByte", new byte[] { 12 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestByteMax", new byte[] { 204, 255 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestDouble", new byte[] { 203, 63, 243, 51, 51, 51, 51, 51, 51 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestDoubleMax", new byte[] { 203, 127, 239, 255, 255, 255, 255, 255, 255 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestFloat", new byte[] { 202, 63, 153, 153, 154 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestFloatMax", new byte[] { 202, 127, 127, 255, 255 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestInt", new byte[] { 12 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestIntMax", new byte[] { 206, 127, 255, 255, 255 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestLong", new byte[] { 12 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestLongMax", new byte[] { 207, 127, 255, 255, 255, 255, 255, 255, 255 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestSByte", new byte[] { 12 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestSByteMax", new byte[] { 127 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestShort", new byte[] { 12 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestShortMax", new byte[] { 205, 127, 255 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestUInt", new byte[] { 12 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestUIntMax", new byte[] { 206, 255, 255, 255, 255 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestULong", new byte[] { 12 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.Primitives.TestULongMax", new byte[] { 207, 255, 255, 255, 255, 255, 255, 255, 255 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestSomethingWithKeyAndID", new byte[] { 146, 123, 163, 68, 97, 110 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestString", new byte[] { 175, 84, 104, 101, 32, 98, 101, 115, 116, 32, 67, 97, 102, 195, 169, 115 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestStringNull", new byte[] { 192 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing0AsConcrete", new byte[] { 146, 163, 68, 97, 110, 123 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing0AsInterface", new byte[] { 146, 0, 146, 163, 68, 97, 110, 123 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing1AsConcrete", new byte[] { 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing1AsInterface", new byte[] { 146, 1, 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing2AsConcrete", new byte[] { 145, 2 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThing2AsInterface", new byte[] { 146, 2, 145, 2 });
            yield return ("MsgPack5.H5.Tests.SharedTestItems.TestThingWrapper", new byte[] { 145, 147, 146, 0, 146, 163, 68, 97, 110, 123, 146, 1, 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192, 146, 2, 145, 2 });
        }
    }
}
