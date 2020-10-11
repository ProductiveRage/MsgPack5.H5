using System;

namespace MessagePack
{
    /// <summary>
    /// This is used when an array of data is being deserialised into an array, rather than being used to instantiate a class or a struct or a more specialised list type
    /// </summary>
    internal sealed class ArrayDataDecoderForArray : ArrayDataDecoderBackedByArray
    {
        public ArrayDataDecoderForArray(Type elementType, uint length, Func<object, Type, object> convert) : base(elementType, length, convert) { }

        public override object GetFinalResult() => _arrayBeingPopulated;
    }
}