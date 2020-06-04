#if H5
using MsgPack5.H5;
#else
using MessagePack;
#endif

namespace SharedTypes
{
    [MessagePackObject]
    public sealed class Thing0 : IThing // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public int Age { get; set; }
    }
}