using System;
#if H5
using MsgPack5.H5;
#else
using MessagePack;
#endif

namespace SharedTypes
{
    [MessagePackObject]
    public sealed class Thing2 : IThing // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public DayOfWeek Day { get; set; }
    }
}