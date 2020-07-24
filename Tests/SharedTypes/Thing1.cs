namespace MessagePack.Tests.SharedTypes
{
    [MessagePackObject]
    public sealed class Thing1 : IThing // Note: Must be public (not internal) to work with MessagePack
    {
        [Key(0)]
        public string[] Roles { get; set; }

        [Key(1)]
        public int[] IDs { get; set; }
    }
}