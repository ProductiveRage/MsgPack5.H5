#if H5
using H5 = MsgPack5.H5;
#else
using NET = MessagePack;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [MessagePackObject]
    public class SomethingWithKeyAndID
    {
        [Key(0)]
        public int Key { get; set; }

        [Key(1)]
        public string ID { get; set; }
    }
}