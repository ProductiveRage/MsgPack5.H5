using System;

namespace MessagePack
{
    /// <summary>
    /// This is used when we're deserialising to a class that is instantiated via a parameterless constructor
    /// </summary>
    internal sealed class ArrayDataDecoderWithNoConstructorParameters : IArrayDataDecoder
    {
        private readonly Func<uint, MemberSummary> _keyedMemberLookup;
        private readonly object _instanceBeingPopulated;
        public ArrayDataDecoderWithNoConstructorParameters(Type type, Func<uint, MemberSummary> keyedMemberLookup)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            var constructor = type.GetConstructor(new Type[0]);
            if (constructor is null)
                throw new ArgumentException("must have an accessible parameterless constructor", nameof(type));

            _keyedMemberLookup = keyedMemberLookup ?? throw new ArgumentNullException(nameof(keyedMemberLookup));
            _instanceBeingPopulated = constructor.Invoke(new object[0]);
        }

        // The "index" here will be from the array of values that we're deserialising - it's possible that there will be more values than there are members (if we're deserialising from an old version of a type to a newer version where members were removed)
        // and so we need to accept that the _keyedMemberLookup may return null and we'll return a default value from GetExpectedTypeForIndex and ignore such a call to SetValueAtIndex. The same situation could happen if there key values on the members on
        // the type have any missing values (if there are [Key(..)] properties that go 0, 1 and then 3, for example - we don't care about the data in slot 2).
        public Type GetExpectedTypeForIndex(uint index) => _keyedMemberLookup(index)?.Type ?? typeof(object);

        public void SetValueAtIndex(uint index, object value)
        {
            var valueToSet = MsgPack5Decoder.TryToCast(value, GetExpectedTypeForIndex(index));
            _keyedMemberLookup(index)?.SetIfWritable(_instanceBeingPopulated, valueToSet);
        }

        public object GetFinalResult() => _instanceBeingPopulated;
    }
}