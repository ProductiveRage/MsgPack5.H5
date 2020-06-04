#if !H5
using MessagePack;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class Wrapper // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public IThing[] Things { get; set; }
    }
}