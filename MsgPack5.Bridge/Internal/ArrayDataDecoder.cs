using System;

namespace MsgPack5.Bridge.Internal
{
    internal sealed class ArrayDataDecoder
    {
        private readonly Func<ulong, Type> _expectedTypeForIndex;
        private readonly Action<ulong, object> _setterForIndex;
        private readonly Func<object> _finalResultGenerator;
        public ArrayDataDecoder(Func<ulong, Type> expectedTypeForIndex, Action<ulong, object> setterForIndex, Func<object> finalResultGenerator)
        {
            _expectedTypeForIndex = expectedTypeForIndex ?? throw new ArgumentNullException(nameof(expectedTypeForIndex));
            _setterForIndex = setterForIndex ?? throw new ArgumentNullException(nameof(setterForIndex));
            _finalResultGenerator = finalResultGenerator ?? throw new ArgumentNullException(nameof(finalResultGenerator));
        }
        public Type GetExpectedTypeForIndex(ulong index) => _expectedTypeForIndex(index);
        public void SetValueAtIndex(ulong index, object value) => _setterForIndex(index, value);
        public object GetFinalResult() => _finalResultGenerator();
    }
}