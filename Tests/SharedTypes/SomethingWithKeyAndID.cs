#if !H5
using MessagePack;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [MessagePackObject]
    public class SomethingWithKeyAndID // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public int Key { get; set; }

        [Key(1)]
        public string ID { get; set; }
    }
}