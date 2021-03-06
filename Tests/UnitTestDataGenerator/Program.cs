﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MessagePack;
using MessagePack.Tests.SharedTestItems;

namespace UnitTestDataGenerator
{
    internal sealed class Program
    {
        private const string _testDataNamespace = "UnitTests";
        private const string _testDataFilename = "TestData.cs";

        private static void Main(string[] args)
        {
            var projectFolderName = args?.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(projectFolderName))
            {
                projectFolderName = Directory.GetCurrentDirectory();
                Console.WriteLine("Expected command line argument to specify project folder location to write Test Data file to - default to build folder " + projectFolderName);
            }
            var projectFolder = new DirectoryInfo(projectFolderName);
            if (!projectFolder.Exists)
            {
                throw new Exception("The specified project folder must already exist but it does not: " + projectFolderName);
            }

            var allTestItemTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && typeof(ITestItem).IsAssignableFrom(t))
                .OrderBy(t => t.FullName);
            var testItemEntries = new List<(string testItemTypeNameLiteral, string byteArrayRepresentation, string alternateResultJson, string errorRepresentation)>();
            foreach (var testItemType in allTestItemTypes)
            {
                var testItem = TestItemInstanceCreator.GetInstance(testItemType.FullName);
                var serialised = MessagePackSerializer.Serialize(type: testItem.SerialiseAs, obj: testItem.Value);
                string alternateResultJson, errorRepresentation;
                try
                {
                    var deserialised = MessagePackSerializer.Deserialize(type: testItem.DeserialiseAs, bytes: serialised);
                    if (!ObjectComparer.AreEqual(deserialised, testItem.Value, out _))
                    {
                        // h5's JsonConvert has a bug where JsonConvert.DeserializeObject<object>("12") will be deserialised into a string instead of an Int64 and this would cause a problem if we were, for example, serialising a byte and then wanting to deserialise
                        // it into an int because the ObjectComparer will be able to tell that they are not precisely the same value and so an alternateResultJson value would be written but this would cause a problem because the h5 Unit Tests code would see that
                        // alternateResultJson and think that the result should be a string (due to that bug). So, to avoid that confusion, if the JSON of the original value matches the JSON of the different value - because that indicates a case that we don't
                        // care about (which should only be primitives).
                        if (JsonSerialiserForComparison.ToJson(deserialised) == JsonSerialiserForComparison.ToJson(testItem.Value))
                            alternateResultJson = null;
                        else
                            alternateResultJson = ToLiteral(JsonSerialiserForComparison.ToJson(deserialised));
                    }
                    else
                        alternateResultJson = "null";
                    errorRepresentation = "null";
                }
                catch (Exception e)
                {
                    // 2020-07-25 DWR: By only checking the top level exception type and its messages, it means that the inner exception content may not be precisely the same but I'm happy with that (eg. when trying to deserialise to a type that has multiple
                    // properties with the same Key then the .NET library will throw a MessagePackSerializationException that has an InnerException that references the FormatterCache`1 and that will have an InnerException that describes the repeated key issue
                    // whereas this library will throw a MessagePackSerializationException with the same message as the C# version but wrap a RepeatedKeyValueException instance - that's close enough to like-for-like behaviour for me)
                    errorRepresentation = $"new ExceptionSummary(TypeRetriever.Get(\"{e.GetType().FullName}\"), {ToLiteral(e.Message)})";
                    alternateResultJson = null;
                }
                testItemEntries.Add((
                    "\"" + testItem.GetType().FullName + "\"",
                    "new byte[] { " + string.Join(", ", serialised) + " }",
                    alternateResultJson,
                    errorRepresentation
                ));
            }

            var content = new StringBuilder();
            content.AppendLine("using System.Collections.Generic;");
            content.AppendLine();
            content.AppendLine("namespace " + _testDataNamespace);
            content.AppendLine("{");
            content.AppendLine("    // This file is generated by the UnitTestDataGenerator project - run that to ensure that this is current (includes all of the tests in the SharedTestItems project) and then run the Unit Tests project");
            content.AppendLine("    internal static class TestData");
            content.AppendLine("    {");
            content.AppendLine("        public static IEnumerable<(string TestItemName, byte[] Serialised, string AlternateResultJson, ExceptionSummary ExpectedError)> GetItems()");
            content.AppendLine("        {");
            foreach (var (testItemTypeNameLiteral, byteArrayRepresentation, alternateResultJson, errorRepresentation) in testItemEntries)
            {
                content.AppendLine($"            yield return ({testItemTypeNameLiteral}, {byteArrayRepresentation}, {alternateResultJson ?? "null"}, {errorRepresentation});");
            }
            content.AppendLine("        }");
            content.AppendLine("    }");
            content.AppendLine("}");

            File.WriteAllText(
                Path.Combine(projectFolder.FullName, _testDataFilename),
                content.ToString()
            );
        }

        private static string ToLiteral(string input)
        {
            // Originally went with https://stackoverflow.com/a/324812/3813189 but it seems like it's not possible to prevent that from wrapping (and adding concatenations) when strings are longer than 80 characters, so instead have tried the
            // below inspired by this comment:
            //
            //  Escaping the string shouldn't be too hard if you use a verbatim string literal (i.e. put @ at the start) - then the only thing you have to do is replace " with ""
            //
            // .. from Jon Skeet on this Stack Overflow answer: https://stackoverflow.com/a/960352/3813189
            return (input == null) ? "null" : $"@\"{input.Replace("\"", "\"\"")}\"";
        }
    }
}