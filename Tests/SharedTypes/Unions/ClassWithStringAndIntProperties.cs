﻿namespace MessagePack.Tests.SharedTypes.Unions
{
    [MessagePackObject]
    public sealed class ClassWithStringAndIntProperties : IUnionExample // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public int Age { get; set; }
    }
}