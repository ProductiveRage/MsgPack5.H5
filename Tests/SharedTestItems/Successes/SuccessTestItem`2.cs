using System;

namespace MessagePack.Tests.SharedTestItems.Successes
{
    /// <summary>
    /// Use this base class to test a successful round trip where a value is serialised as one type and deserialised as another
    /// </summary>
    internal abstract class SuccessTestItem<TSerialiseAs, TDeserialiseAs> : ITestItem
    {
        protected SuccessTestItem(TSerialiseAs value) => Value = value;

        public TSerialiseAs Value { get; }

        public Type SerialiseAs => typeof(TSerialiseAs);
        public Type DeserialiseAs => typeof(TDeserialiseAs);
        object ITestItem.Value => Value;

#if H5
        public virtual Func<MsgPack5DecoderOptions, MsgPack5DecoderOptions> DecodeOptions => null;
#endif
    }
}