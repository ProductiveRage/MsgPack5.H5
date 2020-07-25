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
        public static ArrayDataDecoder GetFor(Type expectedType, uint length)
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
                throw new MessagePackSerializationException(expectedType, new TypeWithoutMessagePackObjectException(expectedType));

            // If the expectedType is NOT an array and it IS a [MessagePack] object then presume that we can dig into its members and look have [Key(..)] attributes and that their values will correspond to indexed values in the array
            if (_decodersForNestedTypes.TryGetValue(expectedType, out var arrayDataDecoder))
                return arrayDataDecoder;
            arrayDataDecoder = GetArrayDecoderWhereArrayElementsAreMembersOfExpectedType(expectedType, length);
            _decodersForNestedTypes[expectedType] = arrayDataDecoder;
            return arrayDataDecoder;
        }

        private static ArrayDataDecoder GetArrayDecoderWhereArrayElementsAreMembersOfExpectedType(Type expectedType, uint length)
        {
            // TODO: Need to handle initialisation-by-constructor (after constructor is called, member setters will still be attempted)
            //
            //  0. If there are [SerializationConstructor] constructors then it's an error case
            //  1. If there is a [SerializationConstructor] constructor then it will attempt to use that, which may result in an exception if the values can not be cast
            //  2. If there is no [SerializationConstructor] constructor then it will look for the constructor where the most parameters can be populated from the available values
            //     - Default value parameters do not appear to be supported by the .NET version
            //     - implicit/explicit operators on types are not supported
            //     - Simple casts are supported; eg. int -> double, string[] -> IEnumerable<string> or List<string>

            var allPublicConstructors = expectedType.GetConstructors();
            if (!allPublicConstructors.Any())
                throw new MessagePackSerializationException(expectedType, new NoAccessibleConstructorsException(expectedType));

            var attributeConstructors = allPublicConstructors.Where(c => c.GetCustomAttributes(typeof(SerializationConstructorAttribute)).Any()).ToArray();
            if (attributeConstructors.Length > 1)
                throw new MessagePackSerializationException(expectedType, new TypeWithMultipleSerializationConstructorsException(expectedType));

            if (attributeConstructors.Length == 1)
                return GetDecoderForNonAmbiguousConstructor(expectedType, attributeConstructors[0], length);

            // If there are zero constructors with a [SerializationConstructor] then we have a few possibilities -
            //  1. There is only one and it is parameterless, this is the easiest (we just call it to get a new instance and then all of the work is done by the member setters)
            //  2. There is only one and it has too many parameters to be satisfied, this is the second easiest (it's a failure case)
            //  3. There is only one and the number of items in the array mean that it's possible that its parameters can be satisfied, this is the third easiest (we try to call it with the available values and see if they are compatible)
            //  4. There are multiple constructors and we have to find the best match, this is the most complicated and expensive because we can only do this when we have the values and can try to see if they fit any if the constructor options
            if (allPublicConstructors.Length == 1)
                return GetDecoderForNonAmbiguousConstructor(expectedType, allPublicConstructors[0], length);

            throw new NotImplementedException("Deserialising where there are multiple constructor options isn't supported yet"); // TODO
        }

        private static ArrayDataDecoder GetDecoderForNonAmbiguousConstructor(Type expectedType, ConstructorInfo constructor, uint numberOfValuesAvailable)
        {
            var constructorParameters = constructor.GetParameters();
            if (!constructorParameters.Any())
            {
                var keyedMembers = GetLookupForKeyMembers(expectedType);
                var initialValue = constructor.Invoke(new object[0]);
                return new ArrayDataDecoder(
                    expectedTypeForIndex: index => keyedMembers(index)?.Type ?? typeof(object), // The "index" here of the array element corresponds to the [Key(..)] attribute value on the expectedType
                    setterForIndex: (index, value) => keyedMembers(index)?.Set(initialValue, value),
                    finalResultGenerator: () => initialValue
                );
            }

            if (constructorParameters.Length > numberOfValuesAvailable)
            {
                // The .NET version doesn't support default constructor parameters, so it doesn't matter if we have enough values for all non-default construcftor parameters, we only need to compare the total number of parameters
                throw new MessagePackSerializationException(expectedType, new MissingValuesForConstructorException(expectedType, numberOfValuesAvailable));
            }

            throw new NotImplementedException("Deserialising via constructor isn't supported yet"); // TODO

            // TODO: Something
            // - Different ArrayDataDecoder configuration where.. something; build a ctor arg list to populate and then list for fields/properties and finally combine by waiting for all
            //   data by calling ctor using arg list and then setting fields/properties after
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
                                throw new MessagePackSerializationException(expectedType, new MemberWithoutKeyOrIgnoreException(expectedType, p));
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
                                throw new MessagePackSerializationException(expectedType, new MemberWithoutKeyOrIgnoreException(expectedType, f));
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
                        throw new MessagePackSerializationException(expectedType, new RepeatedKeyValueException(expectedType, keyedMember.Key, (existingMemberWithSameKey.MemberInfo, keyedMember.MemberSummary.MemberInfo)));
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