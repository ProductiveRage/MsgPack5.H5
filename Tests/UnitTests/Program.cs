using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MsgPack5.H5;
using MsgPack5.H5.Tests.SharedTestItems;
using static H5.Core.dom;

namespace UnitTests
{
    internal static class Program
    {
        private static void Main()
        {
            var allTestItems = TestData.GetItems().ToArray(); // TODO: Try with Take(0), Take(1) and then without to ensure all scenarios ok
            if (!allTestItems.Any())
            {
                document.body.appendChild(GetMessage("There were no tests found to run", isSuccess: false));
                return;
            }

            // If the tests all have names that comes from types within a shared namespace then remove that namespace from the names so that the output is less noisy
            if (allTestItems.Length > 1) // We need at least two tests to find a common namespace!
            {
                var testItemsAndNameSegmentsForThem = allTestItems
                    .Select(testItem => new { NameSegments = testItem.TestItemName.Split('.').ToList(), TestItem = testItem })
                    .ToList();
                while (testItemsAndNameSegmentsForThem.All(entry => entry.NameSegments.Count > 1) && (testItemsAndNameSegmentsForThem.Select(entry => entry.NameSegments.First()).Distinct().Count() == 1))
                {
                    console.log("Remove: " + testItemsAndNameSegmentsForThem.First().NameSegments.First());
                    testItemsAndNameSegmentsForThem.ForEach(entry => entry.NameSegments.RemoveAt(0));
                    console.log("- First now called: " + string.Join(".", testItemsAndNameSegmentsForThem.First().NameSegments));
                } 
                allTestItems = testItemsAndNameSegmentsForThem
                    .OrderBy(entry => entry.NameSegments, TestNameSegmentOrderer.Instance)
                    .Select(entry => (string.Join(".", entry.NameSegments), entry.TestItem.Serialised))
                    .ToArray();
            }

            // TODO: Group the Unit Tests into namespaces (and add tests for failure cases)
            // TODO: Maybe show failures first? Or at least a summary at the top to indicate the numbers of successes and failures?
            var successes = new List<string>();
            var failures = new List<string>();
            foreach (var (testItemName, serialised) in allTestItems)
            {
                var displayName = testItemName; // TODO: Get this properly
                if (ExecuteTest(testItemName, displayName, serialised, document.body))
                    successes.Add(testItemName);
                else
                    failures.Add(testItemName);
            }

            console.log("Number of successes: " + successes.Count);
            foreach (var name in successes)
                console.log("- " + name);

            console.log();

            console.log("Number of failures: " + failures.Count);
            foreach (var name in failures)
                console.log("- " + name);
        }

        private static bool ExecuteTest(string fullName, string displayName, byte[] serialised, HTMLElement appendResultMessageTo)
        {
            try
            {
                var testItem = TestItemInstanceCreator.GetInstance(fullName);
                var decoder = GetNonGenericDecoder(MsgPack5Decoder.Default, testItem.DeserialiseAs);
                var clone = decoder(serialised);
                if (ObjectComparer.AreEqual(testItem.Value, clone))
                {
                    appendResultMessageTo.appendChild(GetMessage(fullName, isSuccess: true));
                    return true;
                }
            }
            catch { }
            appendResultMessageTo.appendChild(GetMessage(fullName, isSuccess: true));
            return true;
        }

        private static Func<byte[], object> GetNonGenericDecoder(MsgPack5Decoder decoder, Type deserialiseAs)
        {
            var unboundGetDecoderMethod = typeof(Program).GetMethod(nameof(GetDecoder), BindingFlags.Static | BindingFlags.NonPublic, parameterTypes: new[] { typeof(MsgPack5Decoder) });
            if (unboundGetDecoderMethod is null)
                throw new Exception("Internal error while trying to retrieve method to form a non-generic Decode call for unit test - this shouldn't be possible");

            var getDecoderMethod = unboundGetDecoderMethod.MakeGenericMethod(deserialiseAs);
            return (Func<byte[], object>)getDecoderMethod.Invoke(null, new[] { decoder });
        }

        private static Func<byte[], object> GetDecoder<T>(MsgPack5Decoder decoder) => serialised => decoder.Decode<T>(serialised);

        private static HTMLElement GetMessage(string text, bool isSuccess)
        {
            var d = new HTMLDivElement { innerText = text };
            d.style.color = "white";
            d.style.backgroundColor = isSuccess ? "#0a0" : "#c00";
            d.style.border = "1px solid " + (isSuccess ? "#080" : "#900");
            d.style.borderRadius = "0.25rem";
            d.style.padding = "0.5rem 1rem";
            d.style.marginBottom = "0.5rem";
            return d;
        }

        private sealed class TestNameSegmentOrderer : IComparer<List<string>>
        {
            public static TestNameSegmentOrderer Instance { get; } = new TestNameSegmentOrderer();
            private TestNameSegmentOrderer() { }

            public int Compare(List<string> x, List<string> y)
            {
                if (x.Count != y.Count)
                    return Comparer<int>.Default.Compare(x.Count, y.Count);

                return Comparer<string>.Default.Compare(
                    JoinSegments(x),
                    JoinSegments(y)
                );

                string JoinSegments(List<string> segments) => string.Join(".", segments);
            }
        }
    }
}