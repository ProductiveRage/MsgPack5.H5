using System;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    /// <summary>
    /// Use this base class to test a successful round trip where a value is serialised as one type and deserialised as another
    /// </summary>
    internal abstract class FailureTestItem<TSerialiseAs, TDeserialiseAs> : ITestItem
    {
        protected FailureTestItem(TSerialiseAs value) => Value = value;

        public TSerialiseAs Value { get; }

        public Type SerialiseAs => typeof(TSerialiseAs);
        public Type DeserialiseAs => typeof(TDeserialiseAs);
        object ITestItem.Value => Value;

#if H5
        public virtual Func<MsgPack5DecoderOptions, MsgPack5DecoderOptions> DecodeOptions => null;
#endif
    }
}