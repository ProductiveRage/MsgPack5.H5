namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// This illustrates that if there is a missing key (if they're non-consecutive) then a null value will be included in the serialised data and it will be ignored during deserialisation
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithNonConsecutiveKeyProperties // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public int Key { get; set; }

        [Key(2)]
        public string ID { get; set; }
    }
}