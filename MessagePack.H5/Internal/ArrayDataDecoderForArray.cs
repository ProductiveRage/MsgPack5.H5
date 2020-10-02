using System;

namespace MessagePack
{
    /// <summary>
    /// This is used when an array of data is being deserialised into an array, rather than being used to instantiate a type
    /// </summary>
    internal sealed class ArrayDataDecoderForArray : IArrayDataDecoder
    {
        private readonly Type _elementType;
        private readonly Func<object, Type, object> _convert;
        private readonly Array _arrayBeingPopulated;
        public ArrayDataDecoderForArray(Type elementType, uint length, Func<object, Type, object> convert)
        {
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            _convert = convert ?? throw new ArgumentNullException(nameof(convert));

            _arrayBeingPopulated = Array.CreateInstance(elementType, (int)length);
        }

        public Type GetExpectedTypeForIndex(uint index) => _elementType;

        public void SetValueAtIndex(uint index, object value)
        {
            var valueToSet = _convert(value, _elementType);
            _arrayBeingPopulated.SetValue(valueToSet, (int)index);
        }

        public object GetFinalResult() => _arrayBeingPopulated;
    }
}