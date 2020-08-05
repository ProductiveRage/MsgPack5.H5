using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    /// <summary>
    /// This illustrates that if we go from a source type that only has one key'd property and we try to deserialise into a type that is instantiated solely by constructor (no settable properties) that it will succeed, so
    /// long as the destination type is 'consistent', meaning that it has key'd properties that correspond to its constructor arguments (so if a property should be removed from the instantiated-via-constructor type then
    /// it can't be fully removed, it has to be kept but should be renamed to make it clear that it's not longer for use and marked Obsolete so that the compiler warns - like ClassWithObsoletedPropertySetByConstructor)
    /// </summary>
    internal sealed class CanNotCompletelyRemovePropertiesIfReliedUponByConstructor : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithNonConsecutiveKeyProperties);
        public Type DeserialiseAs => typeof(ClassWithRemovedPropertySetByConstructor);
        public object Value => new ClassWithNonConsecutiveKeyProperties { Key = 123, ID = "ABC" };
    }
}