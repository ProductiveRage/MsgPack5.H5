using System;

namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// If a type is to instantiated via constructor then it requires an argument for all CONSECUTIVE keys, so you want to remove a property then you can't remove it entirely; you need to maintain it so that the key'd properties
    /// still correspond to the constructor argument list but it's advisable to name it such that it's clear that it's no longer for use and to mark it as Obsolete so that a warning is recorded if it's accessed
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithObsoletedPropertySetByConstructor // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithObsoletedPropertySetByConstructor(int key, object _, string id)
        {
            Key = key;
            ID = id;
        }

        [Key(0)]
        public int Key { get; }

        [Obsolete("This property has been removed and is only still preesnt for backwards compatability - it should never be referenced")]
        [Key(1)]
        public object Removed { get; }

        [Key(2)]
        public string ID { get; }
    }
}