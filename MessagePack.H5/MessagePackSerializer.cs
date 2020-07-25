using static H5.Core.es5;

namespace MessagePack
{
    /// <summary>
    /// This static class uses the MsgPack5Decoder.Default instance and exists to enable calls to be made in a more similar format to MessagePack-CSharp
    /// </summary>
    public static class MessagePackSerializer
    {
        public static T Deserialize<T>(Uint8Array data) => MsgPack5Decoder.Default.Decode<T>(data);
        public static T Deserialize<T>(ArrayBuffer data) => MsgPack5Decoder.Default.Decode<T>(data);
        public static T Deserialize<T>(byte[] data) => MsgPack5Decoder.Default.Decode<T>(data);
        public static T Deserialize<T>(IBuffer data) => MsgPack5Decoder.Default.Decode<T>(data);
    }
}