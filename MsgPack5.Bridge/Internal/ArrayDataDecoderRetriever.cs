using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MsgPack5.Bridge.Internal
{
    internal static class ArrayDataDecoderRetriever
    {
        private static readonly ConcurrentDictionary<Type, ArrayDataDecoder> _decodersForNestedTypes = new ConcurrentDictionary<Type, ArrayDataDecoder>();

        /// <summary>
        /// This returns a type that analyses the target type and returns an object that will allow an array of objects to be read and for them to be used to populate the values in the target instance - if the target instance is an
        /// array then this is a simple task but arrays of values are also used to represent the data for objects where members have Key attributes and, in that case, the returned ArrayDataDecoder will set the individual properties
        /// on the target instance as the values in the array are read from the data stream.
        /// </summary>
        public static ArrayDataDecoder TryToGetFor(Type expectedType, ulong length)
        {
            // TODO: Need to support other types - IEnumerable<T>, List<T>, etc..
            // TODO: Need to handle types that could be initialised with one of the supported types (array, List<T>, etc..) - either via constructor or through implicit/explicit operators(?)
            // TODO: Should support types without ANY attributes? (I don't like the sound of it)

            // If the expectedType is already an array then it's easy to work out how to populate it (this will happen if type param T given to MsgPack5Decoder's Decode was this array type)
            if (expectedType.IsArray)
            {
                if (expectedType.GetArrayRank() != 1)
                    throw new Exception("Can not deserialise to arrays if they're not one dimensional"); // TODO: Are multidimensional arrays supported by MessagePack(-CSharp)? See CountMin type in Mosaik; suggests that they ARE
                var elementType = expectedType.GetElementType();
                var initialValue = Array.CreateInstance(elementType, (long)length);
                return new ArrayDataDecoder(
                    expectedTypeForIndex: _ => elementType,
                    setterForIndex: (index, value) => initialValue.SetValue(value, (long)index),
                    finalResultGenerator: () => initialValue
                );
            }

            // If this isn't a [MessagePack] object then there isn't much more that we can do here
            if (!expectedType.GetCustomAttributes(typeof(MessagePackObjectAttribute)).Any())
                return null;

            // If the expectedType is NOT an array and it IS a [MessagePack] object then presume that we can dig into its members and look have [Key(..)] attributes and that their values will correspond to indexed values in the array
            return _decodersForNestedTypes.GetOrAdd(expectedType, TryToGetArrayDecoderWhereArrayElementsAreMembersOfExpectedType);
        }

        private static ArrayDataDecoder TryToGetArrayDecoderWhereArrayElementsAreMembersOfExpectedType(Type expectedType)
        {
            // TODO: Need to handle initialisation-by-constructor
            // TODO: For this first pass, require parameterless public constructor (and no other public constructor?)

            // TODO: Look for [SerializationConstructor] constructor - no ambiguity, then! (TODO: Have to be public?)
            var attributeConstructors = expectedType.GetConstructors().Where(c => c.GetCustomAttributes<SerializationConstructorAttribute>().Any()).ToArray();
            if (attributeConstructors.Length > 1)
                throw new Exception("Multiple [SerializationConstructor] constructors"); // TODO: More specialised exception type
            else if (attributeConstructors.Length == 1)
            {
                // TODO: Something
                // - Different ArrayDataDecoder configuration where.. something; build a ctor arg list to populate and then list for fields/properties and finally combine by waiting for all
                //   data by calling ctor using arg list and then setting fields/properties after
            }

            var parameterlessConstructor = expectedType.GetConstructor(Type.EmptyTypes);
            if (parameterlessConstructor == null)
                throw new Exception("Currently only support [MessagePackObject] types that may be created via a parameterless constructor");

            var keyedMembers = GetLookupForKeyMembers(expectedType);
            var initialValue = parameterlessConstructor.Invoke(Array.Empty<object>());
            return new ArrayDataDecoder(
                expectedTypeForIndex: index => keyedMembers(index)?.Type ?? typeof(object), // The "index" here of the array element corresponds to the [Key(..)] attribute value on the expectedType
                setterForIndex: (index, value) => keyedMembers(index)?.Set(initialValue, value),
                finalResultGenerator: () => initialValue
            );
        }

        private static Func<ulong, MemberSummary> GetLookupForKeyMembers(Type expectedType)
        {
            var keyedMembers = new Dictionary<ulong, MemberSummary>();
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
                                throw new Exception("All fields and properties in [MessagePackObject] type must have either [Key(..)] or [IgnoreMember] attributes, which is not the case for type " + expectedType); // TODO: Specialised exception type
                            return null;
                        }
                        return new { keyAttribute.Key, Member = new MemberSummary(p.PropertyType, p.Name, instanceAndValueToSet => p.SetValue(instanceAndValueToSet.Instance, instanceAndValueToSet.ValueToSet)) };
                    })
                    .Where(p => p != null);
                var fields = typeToExamine.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Select(f =>
                    {
                        var keyAttribute = (KeyAttribute)f.GetCustomAttributes(typeof(KeyAttribute)).FirstOrDefault();
                        if (keyAttribute == null)
                        {
                            if (!f.GetCustomAttributes(typeof(IgnoreMemberAttribute)).Any())
                                throw new Exception("All fields and properties in [MessagePackObject] type must have either [Key(..)] or [IgnoreMember] attributes, which is not the case for type " + expectedType); // TODO: Specialised exception type
                            return null;
                        }
                        return new { keyAttribute.Key, Member = new MemberSummary(f.FieldType, f.Name, instanceAndValueToSet => f.SetValue(instanceAndValueToSet.Instance, instanceAndValueToSet.ValueToSet)) };
                    })
                    .Where(f => f != null);
                foreach (var keyedMember in properties.Concat(fields))
                {
                    if (keyedMembers.ContainsKey(keyedMember.Key))
                    {
                        // TODO: Consider expanding support of this - if there are multiple members with the same Key value that are of the same type then should be easy enough; if there are
                        // multiple members with the same Key value but the types are compatible then should also be able to support that; might even be able to handle extended interpretations
                        // of "compatible" with implicit/explicit operators?
                        throw new Exception("Repeated Key value encountered when analysing type" + expectedType); // TODO: Make it a more specialised exception type
                    }
                    keyedMembers.Add(keyedMember.Key, keyedMember.Member);
                }
                typeToExamine = typeToExamine.BaseType;
            }
            return index => keyedMembers.TryGetValue(index, out var memberInfo) ? memberInfo : null;
        }

        private sealed class MemberSummary
        {
            private readonly Action<(object Instance, object ValueToSet)> _setter;
            public MemberSummary(Type type, string name, Action<(object Instance, object ValueToSet)> setter)
            {
                Type = type ?? throw new ArgumentNullException(nameof(name));
                Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException($"Null/blank/whitespace-only {nameof(name)} specified");
                _setter = setter ?? throw new ArgumentNullException(nameof(setter));
            }
            public Type Type { get; }
            public string Name { get; }
            public void Set(object instance, object valueToSet) => _setter((instance, valueToSet));
        }
    }
}