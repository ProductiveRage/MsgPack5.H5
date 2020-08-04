namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// This is used to illustrate that writable property setters will be called after the constructor has been called, even if the values that would be passed to the properties have been used in the constructor call (it might seem
    /// feasible that the deserialisation process would keep track of which values in the serialised data had already been used to populate constructor arguments and then NOT use them to set properties but that's not the case.. and
    /// the only way to prove that is to take the value into the constructor and then ignore it - or set it to +1 or something - and then see if the property setter takes precedence after)
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithConstructorThatSetsNothingAndMutableProperty // Note: Must be public (not internal) to work with MessagePack
    {
        [SerializationConstructor]
        public ClassWithConstructorThatSetsNothingAndMutableProperty(int key)
        {
            Key = key + 1; // Set the key to a value OTHER than the original value so that we'll be able to tell whether the property setter was called after the constructor
        }

        [Key(0)]
        public int Key { get; set; }
    }
}