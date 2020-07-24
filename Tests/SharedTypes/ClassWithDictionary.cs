using System.Collections.Generic;

namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithDictionary // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public Dictionary<string, int> Info { get; set; }
    }
}