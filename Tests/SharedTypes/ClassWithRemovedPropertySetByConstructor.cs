namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// If a type is to instantiated via constructor then it requires an argument for all CONSECUTIVE keys, so if a property doesn't exist for a key (like here, there are properties for keys 0 and 2 but not for key 1) then there
    /// still need to be sufficient constructor parameters as if there the were no 'holes' in the keys, even if that means that the argument will be ignored. If a property really need to be removed (and the type relies upon
    /// constructor instantation) then the property should be left in and marked as [Obsolete] and maybe even renamed something to make it clear that it's been removed and should no longer be used.
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithRemovedPropertySetByConstructor // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithRemovedPropertySetByConstructor(int key, object _, string id)
        {
            Key = key;
            ID = id;
        }

        [Key(0)]
        public int Key { get; }

        [Key(2)]
        public string ID { get; }
    }
}