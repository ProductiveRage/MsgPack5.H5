using System;

namespace MessagePack.Tests.SharedTestItems
{
    internal interface ITestItem
    {
        Type SerialiseAs { get; }
        Type DeserialiseAs { get; }
        object Value { get; }
    }
}