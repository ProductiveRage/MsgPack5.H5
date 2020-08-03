using System;

namespace UnitTests
{
    internal static class TypeRetriever
    {
        public static Type Get(string typeName)
        {
            // 2020-08-04 DWR: The type names in the test data don't specify assembly names but they all refer to types in one of the shared projects, which are built as part of the Unit Tests assembly and Type.GetType supports loading types from the
            // currently-executing assembly (or from a core library) if the assembly name isn't specified
            return Type.GetType(typeName) ?? throw new Exception("Unable to retrieve type: " + typeName);
        }
    }
}