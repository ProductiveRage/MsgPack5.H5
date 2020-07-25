using System;

namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithEnumProperty : IUnionExample // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public DayOfWeek Day { get; set; }
    }
}