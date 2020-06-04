﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MessagePack;
using SharedTestItems;

namespace UnitTestDataGenerator
{
    internal sealed class Program
    {
        private const string _testDataNamespace = "MsgPack5.H5.Tests.UnitTests";
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
            var testItemEntries = new List<(string testItemTypeNameLiteral, string byteArrayRepresentation)>();
            foreach (var testItemTypeName in allTestItemTypeNames)
            {
                var testItem = TestItemInstanceCreator.GetInstance(testItemTypeName.FullName);
                var serialised = MessagePackSerializer.Serialize(type: testItem.DeserialiseAs, obj: testItem.Value);
                testItemEntries.Add((
                    "\"" + testItem.GetType().FullName + "\"",
                    "new byte[] { " + string.Join(", ", serialised) + "}"
                ));
            }

            var content = new StringBuilder();
            content.AppendLine("using System.Collections.Generic;");
            content.AppendLine();
            content.AppendLine("namespace " + _testDataNamespace);
            content.AppendLine("{");
            content.AppendLine("    internal static class TestData");
            content.AppendLine("    {");
            content.AppendLine("        public static IEnumerable<(string testItemName, byte[] serialised)> GetItems()");
            content.AppendLine("        {");
            foreach (var (testItemTypeNameLiteral, byteArrayRepresentation) in testItemEntries)
            {
                content.AppendLine($"            yield return ({testItemTypeNameLiteral}, {byteArrayRepresentation});");
            }
            content.AppendLine("        }");
            content.AppendLine("    }");
            content.AppendLine("}");

            File.WriteAllText(
                Path.Combine(projectFolder.FullName, _testDataFilename),
                content.ToString()
            );
       }
    }
}