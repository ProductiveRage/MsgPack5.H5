using System;

namespace MsgPack5.H5.Tests.SharedTestItems
{
    internal interface ITestItem
    {
        Type DeserialiseAs { get; }
        object Value { get; }
    }
}