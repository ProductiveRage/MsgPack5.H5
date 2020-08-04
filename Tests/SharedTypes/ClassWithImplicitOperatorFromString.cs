namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class ClassWithImplicitOperatorFromString // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public string Key { get; set; }

        public static implicit operator ClassWithImplicitOperatorFromString(string value) => (value is null) ? null : new ClassWithImplicitOperatorFromString { Key = value };
    }
}