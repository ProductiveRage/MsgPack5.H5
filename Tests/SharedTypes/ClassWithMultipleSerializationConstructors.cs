namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithMultipleSerializationConstructors // Note: Must be public (not internal) to work with MessagePack
    {
        [SerializationConstructor]
        public ClassWithMultipleSerializationConstructors(int key, string id)
        {
            Key = key;
            ID = id;
        }

        [SerializationConstructor]
        public ClassWithMultipleSerializationConstructors(int key, string id, string additionalInformation) : this(key, id)
        {
            AdditionalInformation = additionalInformation;
        }

        [Key(0)]
        public int Key { get; set; }

        [Key(1)]
        public string ID { get; set; }

        [Key(2)]
        public string AdditionalInformation { get; set; }
    }
}