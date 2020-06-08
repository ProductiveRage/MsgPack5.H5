using H5;
using static H5.Core.es5;

namespace MsgPack5.H5
{
    [External]
    internal static class Uint8ArrayExtensions
    {
        /// <summary>
        /// When calling the BitConverter library functions that take a byte array, we can give them a Uint8Array and they'll be just as happy when executing it as JS - we just need to make it THINK that it's a byte array.
        /// Using this Template attribute approach means that calls to this method don't even appear in the final output, the Uint8Array value is passed directly into where it's needed with zero manipulation or cost.
        /// </summary>
        [Template("{bytes}")]
        public extern static byte[] FreeCastToByteArray(this Uint8Array bytes);
    }
}