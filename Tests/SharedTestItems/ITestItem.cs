using System;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal interface ITestItem
    {
        Type SerialiseAs { get; }
        Type DeserialiseAs { get; }
        object Value { get; }
    }
}