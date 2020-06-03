#if H5
using H5 = MsgPack5.H5;
#else
using System.Collections.Generic;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithDictionary
    {
        [Key(0)]
        public Dictionary<string, int> Info { get; set; }
    }
}