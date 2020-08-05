namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// When deserialised, the constructor with three parameters will not be applicable as the ID property has type string while the constructor paramater is int (and so, since the properties are all read-only, when an instance
    /// of this type is created via the three-parameter constructor and serialised and then deserialised, only the Key property will be populated in the end result)
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithMultipleConstructorsButOnlyShortestIsValid // Note: Must be public (not internal) to work with MessagePack
    {
#if H5
        [Newtonsoft.Json.JsonConstructor] // This is only required for the AlternateResultJson deserialisation in the unit test execution, it won't affect the MessagePack de/serialisation
#endif
        public ClassWithMultipleConstructorsButOnlyShortestIsValid(int key)
        {
            Key = key;
            ID = null;
            Roles = new string[0];
        }

        public ClassWithMultipleConstructorsButOnlyShortestIsValid(int key, int id, string[] roles)
        {
            Key = key;
            ID = id.ToString();
            Roles = roles;
        }

        [Key(0)]
        public int Key { get; }

        [Key(1)]
        public string ID { get; }

        [Key(2)]
        public string[] Roles { get; }
    }
}