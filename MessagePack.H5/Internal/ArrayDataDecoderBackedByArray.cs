using System;

namespace MessagePack
{
    /// <summary>
    /// This base class is used when the source data can be read into an array that can be used to directly provide an instance of the destination type
    /// </summary>
    internal abstract class ArrayDataDecoderBackedByArray : IArrayDataDecoder
    {
        private readonly Type _elementType;
        private readonly Func<object, Type, object> _convert;
        protected readonly Array _arrayBeingPopulated;
        protected ArrayDataDecoderBackedByArray(Type elementType, uint length, Func<object, Type, object> convert)
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

        public abstract object GetFinalResult();
    }
}