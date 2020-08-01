using System.Collections.Generic;

namespace UnitTests
{
    // This file is generated by the UnitTestDataGenerator project - run that to ensure that this is current (includes all of the tests in the SharedTestItems project) and then run the Unit Tests project
    internal static class TestData
    {
        public static IEnumerable<(string TestItemName, byte[] Serialised, ExceptionSummary ExpectedError)> GetItems()
        {
            yield return ("MessagePack.Tests.SharedTestItems.Failures.ConstructorWithTooManyParameters", new byte[] { 145, 123 }, new ExceptionSummary(TypeRetriever.Get("MessagePack.MessagePackSerializationException"), @"Failed to deserialize MessagePack.Tests.SharedTypes.ClassWithTooManyConstructorArguments value."));
            yield return ("MessagePack.Tests.SharedTestItems.Failures.DeserialiseNullIntoInt", new byte[] { 192 }, new ExceptionSummary(TypeRetriever.Get("MessagePack.MessagePackSerializationException"), @"Failed to deserialize System.Int32 value."));
            yield return ("MessagePack.Tests.SharedTestItems.Failures.MissingKeyAttributeInDestinationType", new byte[] { 146, 123, 163, 65, 66, 67 }, new ExceptionSummary(TypeRetriever.Get("MessagePack.MessagePackSerializationException"), @"Failed to deserialize MessagePack.Tests.SharedTypes.ClassWithStringAndIntPropertiesWithMissingKeyAttribute value."));
            yield return ("MessagePack.Tests.SharedTestItems.Failures.MissingMessagePackObjectAttribute", new byte[] { 146, 123, 163, 65, 66, 67 }, new ExceptionSummary(TypeRetriever.Get("MessagePack.MessagePackSerializationException"), @"Failed to deserialize MessagePack.Tests.SharedTypes.ClassThatIsNotMessagePackObject value."));
            yield return ("MessagePack.Tests.SharedTestItems.Failures.MultipleClassWithMultipleSerializationConstructors", new byte[] { 146, 123, 163, 65, 66, 67 }, new ExceptionSummary(TypeRetriever.Get("MessagePack.MessagePackSerializationException"), @"Failed to deserialize MessagePack.Tests.SharedTypes.ClassWithMultipleSerializationConstructors value."));
            yield return ("MessagePack.Tests.SharedTestItems.Failures.NoPublicConstructor", new byte[] { 146, 123, 163, 65, 66, 67 }, new ExceptionSummary(TypeRetriever.Get("MessagePack.MessagePackSerializationException"), @"Failed to deserialize MessagePack.Tests.SharedTypes.ClassWithNoPublicConstructor value."));
            yield return ("MessagePack.Tests.SharedTestItems.Failures.RepeatedTypesInDestinationType", new byte[] { 146, 123, 163, 65, 66, 67 }, new ExceptionSummary(TypeRetriever.Get("MessagePack.MessagePackSerializationException"), @"Failed to deserialize MessagePack.Tests.SharedTypes.ClassWithStringAndIntPropertiesWithRepeatedKeys value."));
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Collections.TestEmptyStringArray", new byte[] { 144 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Collections.TestNullStringArray", new byte[] { 192 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Collections.TestPopulatedStringArray", new byte[] { 146, 163, 97, 98, 99, 163, 100, 101, 102 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.TestClassWithDictionaryProperty", new byte[] { 145, 130, 163, 79, 110, 101, 1, 163, 84, 119, 111, 2 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.TestClassWithStringAndIntProperties", new byte[] { 146, 123, 163, 68, 97, 110 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions.TestClassWithEnumPropertyAsConcrete", new byte[] { 145, 2 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions.TestClassWithEnumPropertyAsInterface", new byte[] { 146, 2, 145, 2 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions.TestClassWithIUnionExampleArrayProperty", new byte[] { 145, 147, 146, 0, 146, 163, 68, 97, 110, 123, 146, 1, 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192, 146, 2, 145, 2 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions.TestClassWithStringAndIntPropertiesAsConcrete", new byte[] { 146, 163, 68, 97, 110, 123 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions.TestClassWithStringAndIntPropertiesAsInterface", new byte[] { 146, 0, 146, 163, 68, 97, 110, 123 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions.TestClassWithStringArrayAndIntArrayPropertiesAsConcrete", new byte[] { 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.MutableClasses.Unions.TestClassWithStringArrayAndIntArrayPropertiesAsInterface", new byte[] { 146, 1, 146, 146, 166, 84, 101, 115, 116, 101, 114, 170, 67, 97, 116, 32, 72, 101, 114, 100, 101, 114, 192 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Nullables.TestDateTimeWithNoValue", new byte[] { 214, 255, 94, 220, 45, 118 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Nullables.TestDateTimeWithValue", new byte[] { 192 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Nullables.TestIntWithNoValue", new byte[] { 192 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Nullables.TestIntWithValue", new byte[] { 42 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.PrimitiveLikes.TestDateTimeMidPrecision", new byte[] { 215, 255, 2, 220, 108, 0, 94, 220, 45, 118 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.PrimitiveLikes.TestDateTimeSimple", new byte[] { 214, 255, 94, 220, 45, 118 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.PrimitiveLikes.TestDateTimeTopPrecision", new byte[] { 199, 12, 255, 0, 183, 27, 0, 0, 0, 0, 5, 93, 229, 47, 118 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.PrimitiveLikes.TestLongerString", new byte[] { 218, 10, 40, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.PrimitiveLikes.TestString", new byte[] { 175, 84, 104, 101, 32, 98, 101, 115, 116, 32, 67, 97, 102, 195, 169, 115 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.PrimitiveLikes.TestStringNull", new byte[] { 192 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestByte", new byte[] { 12 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestByteMax", new byte[] { 204, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestDecimal", new byte[] { 163, 49, 46, 50 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestDecimalMax", new byte[] { 189, 55, 57, 50, 50, 56, 49, 54, 50, 53, 49, 52, 50, 54, 52, 51, 51, 55, 53, 57, 51, 53, 52, 51, 57, 53, 48, 51, 51, 53 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestDecimalMin", new byte[] { 190, 45, 55, 57, 50, 50, 56, 49, 54, 50, 53, 49, 52, 50, 54, 52, 51, 51, 55, 53, 57, 51, 53, 52, 51, 57, 53, 48, 51, 51, 53 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestDouble", new byte[] { 203, 63, 243, 51, 51, 51, 51, 51, 51 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestDoubleMax", new byte[] { 203, 127, 239, 255, 255, 255, 255, 255, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestDoubleMin", new byte[] { 203, 255, 239, 255, 255, 255, 255, 255, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestFloat", new byte[] { 202, 63, 153, 153, 154 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestFloatMax", new byte[] { 202, 127, 127, 255, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestFloatMin", new byte[] { 202, 255, 127, 255, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestInt", new byte[] { 12 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestIntMax", new byte[] { 206, 127, 255, 255, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestIntMin", new byte[] { 210, 128, 0, 0, 0 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestLong", new byte[] { 12 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestLongMax", new byte[] { 207, 127, 255, 255, 255, 255, 255, 255, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestLongMin", new byte[] { 211, 128, 0, 0, 0, 0, 0, 0, 0 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestSByte", new byte[] { 12 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestSByteMax", new byte[] { 127 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestSByteMin", new byte[] { 208, 128 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestShort", new byte[] { 12 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestShortMax", new byte[] { 205, 127, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestShortMin", new byte[] { 209, 128, 0 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestUInt", new byte[] { 12 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestUIntMax", new byte[] { 206, 255, 255, 255, 255 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestULong", new byte[] { 12 }, null);
            yield return ("MessagePack.Tests.SharedTestItems.Successes.Primitives.TestULongMax", new byte[] { 207, 255, 255, 255, 255, 255, 255, 255, 255 }, null);
        }
    }
}
