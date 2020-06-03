#if H5
using H5 = MsgPack5.H5;
#else
using NET = MessagePack;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class Thing0 : IThing
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public int Age { get; set; }
    }
}