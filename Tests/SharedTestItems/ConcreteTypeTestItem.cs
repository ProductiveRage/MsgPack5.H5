using System;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal abstract class ConcreteTypeTestItem<T> : ITestItem
    {
        protected ConcreteTypeTestItem(T value) => Value = value;

        public T Value { get; }

        public Type DeserialiseAs => typeof(T);
        object ITestItem.Value => Value;
    }
}