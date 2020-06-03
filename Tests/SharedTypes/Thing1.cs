#if H5
using H5 = MsgPack5.H5;
#else
using NET = MessagePack;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class Thing1 : IThing
    {
        [Key(0)]
        public string[] Roles { get; set; }

        [Key(1)]
        public int[] IDs { get; set; }
    }
}