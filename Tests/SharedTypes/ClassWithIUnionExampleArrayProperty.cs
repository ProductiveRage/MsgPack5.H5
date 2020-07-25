﻿namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithIUnionExampleArrayProperty // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public IUnionExample[] Items { get; set; }
    }
}