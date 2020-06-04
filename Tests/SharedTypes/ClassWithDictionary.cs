using System.Collections.Generic;
#if H5
using MsgPack5.H5;
#else
using MessagePack;
#endif

namespace SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithDictionary // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public Dictionary<string, int> Info { get; set; }
    }
}