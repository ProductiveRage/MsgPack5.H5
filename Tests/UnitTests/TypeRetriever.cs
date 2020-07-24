using System;

namespace UnitTests
{
    internal static class TypeRetriever
    {
        public static Type Get(string typeName)
        {
            // 2020-07-24 DWR: This MAY need more work in the future but it seemes to work for now (in .NET, Type.GetType will only load types from the currently-executing assembly or from a core library unless the assembly name is specified, which isn't
            // done in the test data here, but this seems happy to load types from the main library assembly into this unit test assembly and so the difference in this behaviour between H5 and .NET works in my favour right now)
            return Type.GetType(typeName) ?? throw new Exception("Unable to retrieve type: " + typeName);
        }
    }
}