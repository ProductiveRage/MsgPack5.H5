#if H5
using H5 = MsgPack5.H5;
#else
using NET = MessagePack;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [Union(0, typeof(Thing0))]
    [Union(1, typeof(Thing1))]
    [Union(2, typeof(Thing2))]
    public interface IThing { }
}