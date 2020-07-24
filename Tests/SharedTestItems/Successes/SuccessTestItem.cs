using System;

namespace MsgPack5.H5.Tests.SharedTestItems.Successes
{
    internal abstract class SuccessTestItem<T> : ITestItem
    {
        protected SuccessTestItem(T value) => Value = value;

        public T Value { get; }

        public Type DeserialiseAs => typeof(T);
        object ITestItem.Value => Value;
    }
}