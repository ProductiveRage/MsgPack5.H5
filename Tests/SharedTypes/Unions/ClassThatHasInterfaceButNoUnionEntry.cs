namespace MessagePack.Tests.SharedTypes.Unions
{
    /// <summary>
    /// This intentionally implements IUnionExample but does NOT have a corresponding Union attribute on that interface
    /// </summary>
    [MessagePackObject]
    public sealed class ClassThatHasInterfaceButNoUnionEntry : IUnionExample // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public int ID { get; set; }
    }
}