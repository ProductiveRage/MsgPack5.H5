using System;
using System.Collections.Generic;

namespace MessagePack
{
    /// <summary>
    /// This non-generic static ArrayDataDecoderForList class is only used to instantiate the generic instance class when the generic type param is only known at runtime
    /// </summary>
    internal static class ArrayDataDecoderForList
    {
        public static IArrayDataDecoder ForType(Type elementType, uint length)
        {
            if (elementType is null)
                throw new ArgumentNullException(nameof(elementType));

            var decoderType = typeof(ArrayDataDecoderForList<>).MakeGenericType(new[] { elementType });
            var constructor = decoderType.GetConstructor(new[] { typeof(Type), typeof(uint) });
            if (constructor is null)
                throw new Exception($"Failed to retrieve expected constructor for {nameof(ArrayDataDecoderForList)} - this shouldn't happen");
            return (IArrayDataDecoder)constructor.Invoke(elementType, length);
        }
    }

    /// <summary>
    /// This is used when an array of data is being deserialised into an List-of-something, rather than being used to instantiate a type (also used for similar types, such as IList-of-something or IReadOnlyList-of-something)
    /// </summary>
    internal sealed class ArrayDataDecoderForList<T> : IArrayDataDecoder
    {
        private readonly Type _elementType;
        private readonly List<T> _listBeingPopulated;
        public ArrayDataDecoderForList(Type elementType, uint length)
        {
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            _listBeingPopulated = new List<T>(capacity: (int)length);

            // Setting a capacity on the list just provides information to it as to how it is expected to grow, it doesn't actually create a list with that many items already in it
            for (var i = 0; i < length; i++)
                _listBeingPopulated.Add(default);
        }

        public Type GetExpectedTypeForIndex(uint index) => _elementType;

        public void SetValueAtIndex(uint index, object value)
        {
            var valueToSet = MsgPack5Decoder.Convert(value, _elementType);
            _listBeingPopulated[(int)index] = H5.Script.Write<T>("{0}", valueToSet);
        }

        public object GetFinalResult() => _listBeingPopulated;
    }
}