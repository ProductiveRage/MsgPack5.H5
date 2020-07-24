using System;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    /// <summary>
    /// Use this base class to test a simple round trip where a value is serialised as one type and deserialised to that same type
    /// </summary>
    internal abstract class SuccessTestItem<T> : ITestItem
    {
        protected SuccessTestItem(T value) => Value = value;

        public T Value { get; }

        public Type SerialiseAs => typeof(T);
        public Type DeserialiseAs => typeof(T);
        object ITestItem.Value => Value;
    }
}