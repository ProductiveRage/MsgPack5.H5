using System;

namespace MessagePack.Tests.SharedTestItems
{
    internal interface ITestItem
    {
        Type SerialiseAs { get; }
        Type DeserialiseAs { get; }
        object Value { get; }
#if H5
        Func<MsgPack5DecoderOptions, MsgPack5DecoderOptions> DecodeOptions { get; }
#endif
    }
}