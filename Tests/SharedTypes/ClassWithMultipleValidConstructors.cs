namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithMultipleValidConstructors // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithMultipleValidConstructors(int key)
        {
            Key = key;
            Name = "Dunno";
            Roles = new string[0];
        }

        public ClassWithMultipleValidConstructors(int key, string name, string[] roles)
        {
            Key = key;
            Name = name;
            Roles = roles;
        }

        [Key(0)]
        public int Key { get; }

        [Key(1)]
        public string Name { get; }

        [Key(2)]
        public string[] Roles { get; }
    }
}