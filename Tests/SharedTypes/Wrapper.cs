#if H5
using H5 = MsgPack5.H5;
#else
using NET = MessagePack;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class Wrapper
    {
        [Key(0)]
        public IThing[] Things { get; set; }
    }
}