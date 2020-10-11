using System;

namespace MessagePack
{
    /// <summary>
    /// This is used when the target is an immutable list type - the toTargetListType delegate will deal with specify how it is instantiated (it might be that there is a a constructor that will accept that the array that we
    /// build up or it might be that it's an immutable type that has a static Empty property and an AddRange instance method that are both used in order to take the temporary array and produce the final result)
    /// </summary>
    internal sealed class ArrayDataDecoderForImmutableList : ArrayDataDecoderBackedByArray
    {
        private readonly Func<Array, object> _toTargetListType;
        public ArrayDataDecoderForImmutableList(Func<Array, object> toTargetListType, Type elementType, uint length, Func<object, Type, object> convert) : base(elementType, length, convert)
        {
            _toTargetListType = toTargetListType ?? throw new ArgumentNullException(nameof(toTargetListType));
        }

        public override object GetFinalResult() => _toTargetListType(_arrayBeingPopulated);
    }
}