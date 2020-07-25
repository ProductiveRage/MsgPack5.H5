using System;

namespace MessagePack
{
    internal interface IArrayDataDecoder
    {
        Type GetExpectedTypeForIndex(uint index);
        void SetValueAtIndex(uint index, object value);
        object GetFinalResult();
    }
}