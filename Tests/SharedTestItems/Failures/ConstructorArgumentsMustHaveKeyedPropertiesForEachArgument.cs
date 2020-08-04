using System;
using MessagePack.Tests.SharedTypes;

namespace MessagePack.Tests.SharedTestItems.Failures
{
    /// <summary>
    /// When trying to deserialise from a type where there is data for key 0 and key 2 but nothing from 1 it will be ok if the target type is constructor-instantiated but has a constructor for all consecutive keys but this target type
    /// only has two constructor arguments but the properties are key'd as 0 and 2 and so the constructor feels unsatisfied because it should have THREE arguments for keys 0 AND 1 and 2
    /// </summary>
    internal sealed class ConstructorArgumentsMustHaveKeyedPropertiesForEachArgument : ITestItem
    {
        public Type SerialiseAs => typeof(ClassWithNonConsecutiveKeyProperties);
        public Type DeserialiseAs => typeof(ClassWithNonConsecutiveKeyPropertiesSetByConstructor);
        public object Value => new ClassWithNonConsecutiveKeyProperties { Key = 123, ID = "ABC" };
    }
}