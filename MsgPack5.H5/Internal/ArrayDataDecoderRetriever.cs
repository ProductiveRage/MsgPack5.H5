using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using H5;

namespace MessagePack
{
    internal static class ArrayDataDecoderRetriever
    {
        private static readonly Dictionary<Type, ArrayDataDecoder> _decodersForNestedTypes = new Dictionary<Type, ArrayDataDecoder>(); // 2020-06-05 DWR: Expect this to be run in the browser so there are no threading issues, so no need for ConcurrentDictionary

        /// <summary>
        /// This returns a type that analyses the target type and returns an object that will allow an array of objects to be read and for them to be used to populate the values in the target instance - if the target instance is an
        /// array then this is a simple task but arrays of values are also used to represent the data for objects where members have Key attributes and, in that case, the returned ArrayDataDecoder will set the individual properties
        /// on the target instance as the values in the array are read from the data stream.
        /// </summary>
        public static ArrayDataDecoder TryToGetFor(Type expectedType, uint length)
        {
            // TODO: Need to support other types - IEnumerable<T>, List<T>, etc..
            // TODO: Need to handle types that could be initialised with one of the supported types (array, List<T>, etc..) - either via constructor or through implicit/explicit operators(?)
            // TODO: Should support types without ANY attributes? (I don't like the sound of it)

            // If the expectedType is already an array then it's easy to work out how to populate it (this will happen if type param T given to MsgPack5Decoder's Decode was this array type)
            if (expectedType.IsArray)
            {
                var getRank = Script.Write<Func<Type, int>>("((typeof(System) !== 'undefined') && System.Array) ? System.Array.getRank : null"); // H5-INTERNALS: The GetRank() method on array types is not available as it is in .NET, so jump into the JS
                if (getRank == null)
                    throw new Exception("No longer have acess to JS-based method System.Array.getRank - did an update of h5 change this?");
                if (getRank(expectedType) != 1)
                    throw new NotSupportedException("Can not deserialise to arrays if they're not one dimensional"); // TODO: Are multidimensional arrays supported by MessagePack(-CSharp)? See CountMin type in Mosaik; suggests that they ARE
                var elementType = expectedType.GetElementType();
                var initialValue = Array.CreateInstance(elementType, (int)length);
                return new ArrayDataDecoder(
                    expectedTypeForIndex: _ => elementType,
                    setterForIndex: (index, value) => initialValue.SetValue(value, (int)index),
                    finalResultGenerator: () => initialValue
                );
            }

            // If this isn't a [MessagePack] object then there isn't much more that we can do here
            if (!expectedType.GetCustomAttributes(typeof(MessagePackObjectAttribute), inherit: false).Any())
                return null;

            // If the expectedType is NOT an array and it IS a [MessagePack] object then presume that we can dig into its members and look have [Key(..)] attributes and that their values will correspond to indexed values in the array
            if (_decodersForNestedTypes.TryGetValue(expectedType, out var arrayDataDecoder))
                return arrayDataDecoder;
            arrayDataDecoder = TryToGetArrayDecoderWhereArrayElementsAreMembersOfExpectedType(expectedType);
            _decodersForNestedTypes[expectedType] = arrayDataDecoder;
            return arrayDataDecoder;
        }

        private static ArrayDataDecoder TryToGetArrayDecoderWhereArrayElementsAreMembersOfExpectedType(Type expectedType)
        {
            // TODO: Need to handle initialisation-by-constructor
            // TODO: For this first pass, require parameterless public constructor (and no other public constructor?)

            // TODO: Look for [SerializationConstructor] constructor - no ambiguity, then! (TODO: Have to be public?)
            var attributeConstructors = expectedType.GetConstructors().Where(c => c.GetCustomAttributes(typeof(SerializationConstructorAttribute)).Any()).ToArray();
            if (attributeConstructors.Length > 1)
                throw new Exception("Multiple [SerializationConstructor] constructors"); // TODO: More specialised exception type
            else if (attributeConstructors.Length == 1)
            {
                // TODO: Something
                // - Different ArrayDataDecoder configuration where.. something; build a ctor arg list to populate and then list for fields/properties and finally combine by waiting for all
                //   data by calling ctor using arg list and then setting fields/properties after
            }

            var parameterlessConstructor = expectedType.GetConstructor(new Type[0]);
            if (parameterlessConstructor == null)
                throw new Exception("Currently only support [MessagePackObject] types that may be created via a parameterless constructor");

            var keyedMembers = GetLookupForKeyMembers(expectedType);
            var initialValue = parameterlessConstructor.Invoke(new object[0]);
            return new ArrayDataDecoder(
                expectedTypeForIndex: index => keyedMembers(index)?.Type ?? typeof(object), // The "index" here of the array element corresponds to the [Key(..)] attribute value on the expectedType
                setterForIndex: (index, value) => keyedMembers(index)?.Set(initialValue, value),
                finalResultGenerator: () => initialValue
            );
        }

        private static Func<uint, MemberSummary> GetLookupForKeyMembers(Type expectedType)
        {
            var keyedMembers = new Dictionary<uint, MemberSummary>();
            var typeToExamine = expectedType;
            while (typeToExamine != null)
            {
                var properties = typeToExamine.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite && !p.GetIndexParameters().Any())
                    .Select(p =>
                    {
                        var keyAttribute = (KeyAttribute)p.GetCustomAttributes(typeof(KeyAttribute)).FirstOrDefault();
                        if (keyAttribute == null)
                        {
                            if (!p.GetCustomAttributes(typeof(IgnoreMemberAttribute)).Any())
                                throw new MemberWithoutKeyOrIgnoreException(expectedType, p);
                            return null;
                        }
                        return new { keyAttribute.Key, MemberSummary = new MemberSummary(p.PropertyType, p, instanceAndValueToSet => p.SetValue(instanceAndValueToSet.Instance, instanceAndValueToSet.ValueToSet)) };
                    })
                    .Where(p => p != null);
                var fields = typeToExamine.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Select(f =>
                    {
                        var keyAttribute = (KeyAttribute)f.GetCustomAttributes(typeof(KeyAttribute)).FirstOrDefault();
                        if (keyAttribute == null)
                        {
                            if (!f.GetCustomAttributes(typeof(IgnoreMemberAttribute)).Any())
                                throw new MemberWithoutKeyOrIgnoreException(expectedType, f);
                            return null;
                        }
                        return new { keyAttribute.Key, MemberSummary = new MemberSummary(f.FieldType, f, instanceAndValueToSet => f.SetValue(instanceAndValueToSet.Instance, instanceAndValueToSet.ValueToSet)) };
                    })
                    .Where(f => f != null);
                foreach (var keyedMember in properties.Concat(fields))
                {
                    if (keyedMembers.TryGetValue(keyedMember.Key, out var existingMemberWithSameKey))
                    {
                        // TODO: Consider expanding support of this - if there are multiple members with the same Key value that are of the same type then should be easy enough; if there are
                        // multiple members with the same Key value but the types are compatible then should also be able to support that; might even be able to handle extended interpretations
                        // of "compatible" with implicit/explicit operators?
                        throw new RepeatedKeyValueException(expectedType, keyedMember.Key, (existingMemberWithSameKey.MemberInfo, keyedMember.MemberSummary.MemberInfo));
                    }
                    keyedMembers.Add(keyedMember.Key, keyedMember.MemberSummary);
                }
                typeToExamine = typeToExamine.BaseType;
            }
            return index => keyedMembers.TryGetValue(index, out var memberInfo) ? memberInfo : null;
        }

        private sealed class MemberSummary
        {
            private readonly Action<(object Instance, object ValueToSet)> _setter;
            public MemberSummary(Type type, MemberInfo memberInfo, Action<(object Instance, object ValueToSet)> setter)
            {
                Type = type ?? throw new ArgumentNullException(nameof(type));
                MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
                _setter = setter ?? throw new ArgumentNullException(nameof(setter));
            }
            public Type Type { get; }
            public MemberInfo MemberInfo { get; }
            public void Set(object instance, object valueToSet) => _setter((instance, valueToSet));
        }
    }
}