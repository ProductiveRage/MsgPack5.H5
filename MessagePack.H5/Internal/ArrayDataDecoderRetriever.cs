using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MessagePack
{
    internal static class ArrayDataDecoderRetriever
    {
        private static readonly Dictionary<Type, Func<IArrayDataDecoder>> _decoderBuildersForNestedTypes = new Dictionary<Type, Func<IArrayDataDecoder>>(); // 2020-06-05 DWR: Expect this to be run in the browser so there are no threading issues, so no need for ConcurrentDictionary

        /// <summary>
        /// This returns a type that analyses the target type and returns an object that will allow an array of objects to be read and for them to be used to populate the values in the target instance - if the target instance is an
        /// array then this is a simple task but arrays of values are also used to represent the data for objects where members have Key attributes and, in that case, the returned ArrayDataDecoder will set the individual properties
        /// on the target instance as the values in the array are read from the data stream.
        /// </summary>
        public static IArrayDataDecoder GetFor(Type expectedType, uint length, Func<object, Type, object> convert)
        {
            if (expectedType is null)
                throw new ArgumentNullException(nameof(expectedType));
            if (convert is null)
                throw new ArgumentNullException(nameof(convert));

            // TODO: MessagePack-CSharp also handles "Custom implementations of ICollection<> or IDictionary<,> with a parameterless constructor" and "Custom implementations of ICollection or IDictionary with a parameterless constructor"

            // If the expectedType is already an array then it's easy to work out how to populate it (this will happen if type param T given to MsgPack5Decoder's Decode was this array type)
            if (expectedType.IsArray)
            {
                var getRank = H5.Script.Write<Func<Type, int>>("((typeof(System) !== 'undefined') && System.Array) ? System.Array.getRank : null"); // H5-INTERNALS: The GetRank() method on array types is not available as it is in .NET, so jump into the JS
                if (getRank == null)
                    throw new Exception("No longer have acess to JS-based method System.Array.getRank - did an update of h5 change this?");
                if (getRank(expectedType) != 1)
                    throw new NotSupportedException("Can not deserialise to arrays if they're not one dimensional"); // TODO: Need to handle multi-dimensional arrays as specified in https://github.com/neuecc/MessagePack-CSharp#built-in-supported-types
                return new ArrayDataDecoderForArray(expectedType.GetElementType(), length, convert);
            }

            // Handle the common BCL generic list types
            if (expectedType.IsGenericType)
            {
                // For IEnumerable<T> target, we can just use an array to satisfy it as the caller doesn't care what concrete type that it is
                if (expectedType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    var elementType = expectedType.GetGenericArguments()[0];
                    return new ArrayDataDecoderForArray(elementType, length, convert);
                }

                // For all of these types, we can use a List<T>-emitting decoder
                if ((expectedType.GetGenericTypeDefinition() == typeof(List<>)) || (expectedType.GetGenericTypeDefinition() == typeof(IList<>)) || (expectedType.GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    var elementType = expectedType.GetGenericArguments()[0];
                    return ArrayDataDecoderForList.ForType(elementType, length, convert);
                }
            }

            // If the target ISN'T a super-common BCL list type then try a couple of other approaches if it LOOKS like a list type (if it implements IEnumerable-of-something and only if there is a SINGLE IEnumerable-of-something
            // interface because, otherwise, we don't know which is the item type of the list; this applies to types like ImmutableList<T> and might work with ImmutableDictionary<TKey, TValue> since that class only implements
            // IEnumerable<KeyValuePair<TKey, TValue>>, note that it doesn't matter if it implements the non-generic IEnumerable interface; we'll ignore that)
            //
            //  1. See if there is a constructor that will accept an IEnumerable-of-something and use that to instantiate the type
            //  2. See if there is a static "Empty" property and an "AddRange" method that takes an IEnumerable-of-something and use those to instantiate the type
            //
            // TODO [2020-10-02 DWR]: Would there be any benefit to caching these type generators? Need to do some profiling since reflection is so different in h5 compared to .NET
            var enumerableOfSomethingImplementations = expectedType
                .GetInterfaces()
                .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                .ToArray();
            if (enumerableOfSomethingImplementations.Length == 1)
            {
                // Approach 1: Look for a constructor that will take an IEnumerable-of-something
                var elementType = enumerableOfSomethingImplementations[0].GetGenericArguments()[0];
                var constructorParameterTypeToOffer = elementType.MakeArrayType(); // By trying to find a constructor that will take an array, we're covering the cases of constructors that take arrays AND that take IEnumerable<T> (TODO: Add unit tests for both scenarios)
                var correspondingConstructor = expectedType
                    .GetConstructors()
                    .FirstOrDefault(ctor =>
                    {
                        var ctorParams = ctor.GetParameters();
                        if (!ctorParams.Any())
                            return false;

                        if (!ctorParams[0].ParameterType.IsAssignableFrom(constructorParameterTypeToOffer))
                            return false;

                        return (ctorParams.Length == 1) || ctorParams.Skip(1).All(p => p.IsOptional);
                    });
                if (correspondingConstructor is object)
                    return new ArrayDataDecoderForImmutableList(toAdd => correspondingConstructor.Invoke(arguments: new object[] { toAdd }), elementType, length, convert);

                // Approach 2: Look for a static Empty property that and an instance AddRange method that takes an IEnumerable-of-something
                var emptyProperty = expectedType.GetProperty("Empty", BindingFlags.Public | BindingFlags.Static);
                if ((emptyProperty is object) && !emptyProperty.GetIndexParameters().Any())
                {
                    var addRangeMethod = expectedType.GetMethod("AddRange", BindingFlags.Public | BindingFlags.Instance, new[] { typeof(IEnumerable<>).MakeGenericType(new[] { elementType }) });
                    if ((addRangeMethod is object) && expectedType.IsAssignableFrom(addRangeMethod.ReturnType))
                    {
                        var emptyInstance = emptyProperty.GetValue(null);
                        return new ArrayDataDecoderForImmutableList(toAdd => addRangeMethod.Invoke(emptyInstance, new object[] { toAdd }), elementType, length, convert);
                    }
                }
            }

            // If this isn't a [MessagePack] object then there isn't much more that we can do here
            if (!expectedType.GetCustomAttributes(typeof(MessagePackObjectAttribute), inherit: false).Any())
                throw new MessagePackSerializationException(expectedType, new TypeWithoutMessagePackObjectException(expectedType));

            // If the expectedType is NOT an array and it IS a [MessagePack] object then presume that we can dig into its members and look have [Key(..)] attributes and that their values will correspond to indexed values in the array
            if (!_decoderBuildersForNestedTypes.TryGetValue(expectedType, out var arrayDataDecoderBuilder))
            {
                arrayDataDecoderBuilder = GetArrayDecoderBuilderWhereArrayElementsAreMembersOfExpectedType(expectedType, convert);
                _decoderBuildersForNestedTypes[expectedType] = arrayDataDecoderBuilder;
            }
            return arrayDataDecoderBuilder();
        }

        /// <summary>
        /// This is used where the expectedType is a class or struct where the members of the source array correspond to members (and/or constructor parameters) for it
        /// </summary>
        private static Func<IArrayDataDecoder> GetArrayDecoderBuilderWhereArrayElementsAreMembersOfExpectedType(Type expectedType, Func<object, Type, object> convert)
        {
            var allPublicConstructors = expectedType.GetConstructors();
            if (!allPublicConstructors.Any())
                throw new MessagePackSerializationException(expectedType, new NoAccessibleConstructorsException(expectedType));

            var attributeConstructors = allPublicConstructors.Where(c => c.GetCustomAttributes(typeof(SerializationConstructorAttribute)).Any()).ToArray();
            if (attributeConstructors.Length > 1)
                throw new MessagePackSerializationException(expectedType, new TypeWithMultipleSerializationConstructorsException(expectedType));

            if (attributeConstructors.Length == 1)
                return GetDecoderBuilderForNonAmbiguousConstructor(expectedType, attributeConstructors[0], convert);

            // Try to find the best constructor - that's the one with the most parameters that can be satisfied by the key'd members (there need to be consecutive key'd members for each of the constructor parameters and the types of the members
            // have to either precisely match the parameter types or be assignable to them - for example, an int[] member may be used to populate an IEnumerable<int> parameter but an int member may NOT be used to populate a double parameter)
            var (keyedMemberLookup, maxConsecutiveKey, maxKey) = GetLookupForKeyMembers(expectedType);
            var numberOfConsecutiveKeyMembers = maxConsecutiveKey.HasValue ? (maxConsecutiveKey.Value + 1) : 0;
            foreach (var (constructor, constructorParameters) in allPublicConstructors.Select(publicConstructor => (publicConstructor, publicConstructor.GetParameters())).OrderByDescending(entry => entry.Item2.Length))
            {
                if (!constructorParameters.Any())
                {
                    // If we've ended up considering a parameterless constructor then life is easy, there are no further checks to perform
                    return GetDecoderBuilderForNonAmbiguousConstructor(expectedType, constructor, convert);
                }

                if (constructorParameters.Length > numberOfConsecutiveKeyMembers)
                {
                    // There must be key'd members for each parameter of the constructor - if the constructor takes three arguments and there are two key'd members that have index values 0 and 2 then that's not sufficient, we can't take assume
                    // a default value for the missing index 1 value when trying to provider constructor arguments
                    continue;
                }

                if (constructorParameters.Select((parameter, index) => parameter.ParameterType.IsAssignableFrom(keyedMemberLookup((uint)index).Type)).All(isMatch => isMatch))
                {
                    // If all of the constructor parameters can be satisfied by key'd members then we've found our best match
                    return GetDecoderBuilderForNonAmbiguousConstructor(expectedType, constructor, convert);
                }
            }

            // If we got here then we couldn't find a constructor that we could use to deserialise an instance of destination type :(
            throw new MessagePackSerializationException(expectedType, new NoCompatibleConstructorFoundException(expectedType));
        }

        private static Func<IArrayDataDecoder> GetDecoderBuilderForNonAmbiguousConstructor(Type expectedType, ConstructorInfo constructor, Func<object, Type, object> convert)
        {
            var (keyedMemberLookup, maxConsecutiveKey, maxKey) = GetLookupForKeyMembers(expectedType);

            var constructorParameters = constructor.GetParameters();
            if (!constructorParameters.Any())
                return () => new ArrayDataDecoderWithNoConstructorParameters(expectedType, keyedMemberLookup, convert);

            // In the .NET library, if a type that is instantiated by constructor is serialised and it had three properties and is then extended to add a new property then this will succeed with the original serialised content so
            // long as the new number of key'd members on the type is matched by the number of constructor parameters. So we could serialise an older version with three properties and successfully deserialise it into a newer version
            // that has properties, so long as the new version has four key'd members and four corresponding constructor parameters.
            // - Note: Constructors are only supported if there are key'd members that correspond to all of the arguments, so if there are only key'd members with index value 0 and 2 then a constructor with three arguments can NOT be
            //         used because there is no key index 1 member on the type
            var numberOfValuesAvailableForConstructorParameters = maxConsecutiveKey.HasValue ? (maxConsecutiveKey.Value + 1) : 0;
            if (constructorParameters.Length > numberOfValuesAvailableForConstructorParameters)
            {
                // The .NET version doesn't support default constructor parameters, so it doesn't matter if we have enough values for all non-default construcftor parameters, we only need to compare the total number of parameters
                throw new MessagePackSerializationException(expectedType, new MissingValuesForConstructorException(expectedType, numberOfValuesAvailableForConstructorParameters));
            }

            // Note: we know that maxKey will have a value here because.. TODO: Something about otherwise the .NET library would not support the type for de/serialising because the keyed members wouldn't match the constructor parameter count
            return () => new ArrayDataDecoderWithParameteredConstructor(constructor, keyedMemberLookup, maxKey.Value, convert);
        }

        private static (Func<uint, MemberSummary> lookup, uint? maxConsecutiveKey, uint? maxKey) GetLookupForKeyMembers(Type expectedType)
        {
            var keyedMembers = new Dictionary<uint, MemberSummary>();
            var typeToExamine = expectedType;
            while (typeToExamine != null)
            {
                var properties = typeToExamine.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => !p.GetIndexParameters().Any())
                    .Select(p =>
                    {
                        var keyAttribute = (KeyAttribute)p.GetCustomAttributes(typeof(KeyAttribute)).FirstOrDefault();
                        if (keyAttribute == null)
                        {
                            if (!p.GetCustomAttributes(typeof(IgnoreMemberAttribute)).Any())
                                throw new MessagePackSerializationException(expectedType, new MemberWithoutKeyOrIgnoreException(expectedType, p));
                            return null;
                        }
                        Action<(object Instance, object ValueToSet)> setterIfWritable;
                        if (p.CanWrite)
                            setterIfWritable =  instanceAndValueToSet => p.SetValue(instanceAndValueToSet.Instance, instanceAndValueToSet.ValueToSet);
                        else
                            setterIfWritable = null;
                        return new { keyAttribute.Key, MemberSummary = new MemberSummary(p.PropertyType, p, setterIfWritable)};
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
                        // multiple members with the same Key value but the types are compatible then should also be able to support that (need to check what the .NET library does in this case)
                        throw new MessagePackSerializationException(expectedType, new RepeatedKeyValueException(expectedType, keyedMember.Key, (existingMemberWithSameKey.MemberInfo, keyedMember.MemberSummary.MemberInfo)));
                    }
                    keyedMembers.Add(keyedMember.Key, keyedMember.MemberSummary);
                }
                typeToExamine = typeToExamine.BaseType;
            }

            // The maximum CONSECUTIVE key is important when trying to find the best constructor - if the keys are non-consecutive then only the ones that ARE may be used to satisfy constructor parameters
            var allKeys = keyedMembers.Keys;
            uint? maxConsecutiveKey = null;
            foreach (var key in allKeys.OrderBy(key => key))
            {
                if (key == 0)
                {
                    maxConsecutiveKey = key;
                    continue;
                }
                if (maxConsecutiveKey.HasValue && (key == (maxConsecutiveKey + 1)))
                {
                    maxConsecutiveKey = key;
                    continue;
                }
                break;
            }

            return (
                index => keyedMembers.TryGetValue(index, out var memberInfo) ? memberInfo : null,
                maxConsecutiveKey,
                allKeys.Any() ? allKeys.Max() : (uint?)null
            );
        }
    }
}