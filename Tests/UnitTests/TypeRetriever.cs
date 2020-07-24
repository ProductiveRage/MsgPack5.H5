using System;

namespace UnitTests
{
    internal static class TypeRetriever
    {
        public static Type Get(string typeName)
        {
            // TODO [2020-07-24 DWR]: Implement this properly (probably makes sense to change the "MsgPack.H5" namespace to "MessagePack")

            const string ifStartsWith = "MessagePack.";
            const string thenReplaceWith = "MsgPack5.H5.";
            if (typeName.StartsWith(ifStartsWith))
                typeName = thenReplaceWith + typeName.Substring(ifStartsWith.Length);

            return Type.GetType(typeName) ?? throw new Exception("Unable to retrieve type: " + typeName);
        }
    }
}