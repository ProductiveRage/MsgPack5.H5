#if H5
using MsgPack5.H5;
#else
using MessagePack;
#endif

namespace SharedTypes
{
    [Union(0, typeof(Thing0))]
    [Union(1, typeof(Thing1))]
    [Union(2, typeof(Thing2))]
    public interface IThing { } // Note: Must be public (not internal) to work with MessagePack
}