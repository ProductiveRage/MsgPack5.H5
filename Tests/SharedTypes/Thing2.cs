#if H5
using H5 = MsgPack5.H5;
#else
using System;
#endif

namespace MsgPack5.H5.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class Thing2 : IThing
    {
        [Key(0)]
        public DayOfWeek Day { get; set; }
    }
}