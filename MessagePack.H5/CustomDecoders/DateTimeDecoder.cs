using System;

namespace MessagePack
{
    // Logic here courtesy of https://github.com/neuecc/MessagePack-CSharp
    public sealed class DateTimeDecoder : ICustomDecoder // This is public, rather than internal, in case consuming code wants to use this in conjunction with other custom decoders
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private const long BclSecondsAtUnixEpoch = 62135596800;
        private const int NanosecondsPerTick = 100;

        public static DateTimeDecoder Instance { get; } = new DateTimeDecoder();
        private DateTimeDecoder() { }

        public MsgPack5Decoder.Decoder TryToGetDecoder(sbyte typeCode)
        {
            if (typeCode != -1) // This is the magic code for DateTime values from MessagePack-CSharp
                return null;

            return (buffer, expectedType) =>
            {
                switch (buffer.Length)
                {
                    case 4:
                        return UnixEpoch.AddSeconds(buffer.ReadUInt32BE(0));

                    case 8:
                        {
                            var ulongValue = buffer.ReadUInt64BE(0);
                            var nanoseconds = (long)(ulongValue >> 34);
                            var seconds = ulongValue & 0x00000003ffffffff;
                            return UnixEpoch.AddSeconds(seconds).AddTicks(nanoseconds / NanosecondsPerTick);
                        }

                    case 12:
                        {
                            var nanoseconds = buffer.ReadUInt32BE(0);
                            var longValue = buffer.ReadInt64BE(4);
                            return UnixEpoch.AddSeconds(longValue).AddTicks(nanoseconds / NanosecondsPerTick);
                        }

                    default: throw new InvalidOperationException($"Length of extension was {buffer.Length} and this is not an expected value for the {nameof(DateTimeDecoder)}");
                }
            };
        }
    }
}