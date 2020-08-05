using System.Collections.Generic;
using System.Linq;

namespace MessagePack.Tests.SharedTypes
{
    /// <summary>
    /// The property type doesn't precisely match the constructor parameter type but it IS a type that is assignable to the parameter type and so all should be good
    /// </summary>
    [MessagePackObject]
    public sealed class ClassWithConstructorWhoseParameterTypeIsAssignableFromProperty // Note: Must be public (not internal) to work with MessagePack
    {
        public ClassWithConstructorWhoseParameterTypeIsAssignableFromProperty(IEnumerable<int> values) => values?.ToArray();

        [Key(0)]
        public int[] Values { get; }
    }
}