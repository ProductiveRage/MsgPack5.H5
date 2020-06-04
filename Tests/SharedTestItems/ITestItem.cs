using System;

namespace SharedTestItems
{
    internal interface ITestItem
    {
        Type DeserialiseAs { get; }
        object Value { get; }
    }
}