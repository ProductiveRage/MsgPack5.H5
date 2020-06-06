using System;

namespace MsgPack5.H5
{
    internal sealed class ArrayDataDecoder
    {
        private readonly Func<uint, Type> _expectedTypeForIndex;
        private readonly Action<uint, object> _setterForIndex;
        private readonly Func<object> _finalResultGenerator;
        public ArrayDataDecoder(Func<uint, Type> expectedTypeForIndex, Action<uint, object> setterForIndex, Func<object> finalResultGenerator)
        {
            _expectedTypeForIndex = expectedTypeForIndex ?? throw new ArgumentNullException(nameof(expectedTypeForIndex));
            _setterForIndex = setterForIndex ?? throw new ArgumentNullException(nameof(setterForIndex));
            _finalResultGenerator = finalResultGenerator ?? throw new ArgumentNullException(nameof(finalResultGenerator));
        }
        public Type GetExpectedTypeForIndex(uint index) => _expectedTypeForIndex(index);
        public void SetValueAtIndex(uint index, object value) => _setterForIndex(index, value);
        public object GetFinalResult() => _finalResultGenerator();
    }
}