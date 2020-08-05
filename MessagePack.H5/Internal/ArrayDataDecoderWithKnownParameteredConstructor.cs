using System;
using System.Reflection;

namespace MessagePack
{
    /// <summary>
    /// This is used when we're deserialising to a type via a constructor where that constructor is known and which not at least one parameter
    /// </summary>
    internal sealed class ArrayDataDecoderWithKnownParameteredConstructor : IArrayDataDecoder
    {
        private readonly ConstructorInfo _constructor;
        private readonly Func<uint, MemberSummary> _keyedMemberLookup;
        private readonly uint _maxKey;
        private readonly ParameterInfo[] _constructorParameters;
        private readonly object[] _arrayBeingPopulated;
        public ArrayDataDecoderWithKnownParameteredConstructor(ConstructorInfo constructor, Func<uint, MemberSummary> keyedMemberLookup, uint maxKey)
        {
            _constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
            _constructorParameters = constructor.GetParameters();
            _keyedMemberLookup = keyedMemberLookup ?? throw new ArgumentNullException(nameof(keyedMemberLookup));
            _maxKey = maxKey;
            _arrayBeingPopulated = new object[(int)(_maxKey + 1)];
        }

        // The "index" here will be from the array of values that we're deserialising - it's possible that there will be more values than there are members (if we're deserialising from an old version of a type to a newer version where members were removed)
        // and so we need to accept that the _keyedMemberLookup may return null and we'll return a default value from GetExpectedTypeForIndex (though we may still add the value to the _arrayBeingPopulated in case we need it for the constructor, so long as
        // the index doesn't exceed the number of key'd members / constructor arguments that we know that we have to capture). The same situation could happen if there key values on the members on the type have any missing values (if there are [Key(..)]
        // properties that go 0, 1 and then 3, for example - we don't care about the data in slot 2).
        // TODO: Test whether "holes" in the key indexes require constructor "placeholder" parameters or not
        public Type GetExpectedTypeForIndex(uint index) => _keyedMemberLookup(index)?.Type ?? typeof(object); // TODO: This needs to consider constructor parameters as well.. or should it have check that those were compatible before getting this far??

        public void SetValueAtIndex(uint index, object value)
        {
            if (index <= _maxKey)
            {
                var requiredType = (index < _constructorParameters.Length)
                    ? _constructorParameters[index].ParameterType
                    : typeof(object);
                var valueToSet = MsgPack5Decoder.Convert(value, requiredType);
                _arrayBeingPopulated.SetValue(valueToSet, (int)index);
            }
        }

        public object GetFinalResult()
        {
            var instance = _constructor.Invoke(_arrayBeingPopulated);
            for (uint index = 0; index <= _maxKey; index++)
            {
                var valueToSet = MsgPack5Decoder.Convert(_arrayBeingPopulated[(int)index], GetExpectedTypeForIndex(index));
                _keyedMemberLookup(index)?.SetIfWritable(instance, valueToSet);
            }
            return instance;
        }
    }
}