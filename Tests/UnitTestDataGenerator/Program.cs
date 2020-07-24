﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MessagePack;
using MsgPack5.H5.Tests.SharedTestItems;

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

            var allTestItemTypeNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && typeof(ITestItem).IsAssignableFrom(t))
                .OrderBy(t => t.FullName);
            var testItemEntries = new List<(string testItemTypeNameLiteral, string byteArrayRepresentation, string errorRepresentation)>();
            foreach (var testItemTypeName in allTestItemTypeNames)
            {
                var testItem = TestItemInstanceCreator.GetInstance(testItemTypeName.FullName);
                var serialised = MessagePackSerializer.Serialize(type: testItem.SerialiseAs, obj: testItem.Value);
                string errorRepresentation;
                try
                {
                    MessagePackSerializer.Deserialize(type: testItem.DeserialiseAs, bytes: serialised);
                    errorRepresentation = "null";
                }
                catch (Exception e)
                {
                    errorRepresentation = $"new ExceptionSummary(TypeRetriever.Get(\"{e.GetType().FullName}\"), {ToLiteral(e.Message)})";
                }
                testItemEntries.Add((
                    "\"" + testItem.GetType().FullName + "\"",
                    "new byte[] { " + string.Join(", ", serialised) + " }",
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
            content.AppendLine("        public static IEnumerable<(string TestItemName, byte[] Serialised, ExceptionSummary ExpectedError)> GetItems()");
            content.AppendLine("        {");
            foreach (var (testItemTypeNameLiteral, byteArrayRepresentation, errorRepresentation) in testItemEntries)
            {
                content.AppendLine($"            yield return ({testItemTypeNameLiteral}, {byteArrayRepresentation}, {errorRepresentation});");
            }
            content.AppendLine("        }");
            content.AppendLine("    }");
            content.AppendLine("}");

            File.WriteAllText(
                Path.Combine(projectFolder.FullName, _testDataFilename),
                content.ToString()
            );
        }

        // Courtesy of https://stackoverflow.com/a/324812/3813189
        private static string ToLiteral(string input)
        {
            using var writer = new StringWriter();
            using var provider = CodeDomProvider.CreateProvider("CSharp");
            provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
            return writer.ToString();
        }
    }
}