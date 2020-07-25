﻿using System;

namespace MessagePack
{
    /// <summary>
    /// This is used when an array of data is being deserialised into an array, rather than being used to instantiate a type
    /// </summary>
    internal sealed class ArrayDataDecoderForArray : IArrayDataDecoder
    {
        private readonly Type _elementType;
        private readonly uint _length;
        private readonly Array _arrayBeingPopulated;
        public ArrayDataDecoderForArray(Type elementType, uint length)
        {
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            _arrayBeingPopulated = Array.CreateInstance(elementType, (int)length);
        }
        public Type GetExpectedTypeForIndex(uint index) => _elementType;
        public void SetValueAtIndex(uint index, object value) => _arrayBeingPopulated.SetValue(value, (int)index);
        public object GetFinalResult() => _arrayBeingPopulated;
    }
}