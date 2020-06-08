using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MsgPack5.H5;
using MsgPack5.H5.Tests.SharedTestItems;
using static H5.Core.dom;

namespace UnitTests
{
    internal static class Program
    {
        private static async Task Main()
        {
            var allTestItems = GetTestsToRun().ToArray();
            if (!allTestItems.Any())
            {
                document.body.appendChild(GetMessage("There were no tests found to run", isSuccess: false));
                return;
            }

            var (ToDisplay, SetStatus, SetSuccessCount, SetFailureCount) = GetRunningSummary(allTestItems.Length);
            document.body.appendChild(ToDisplay);

            // TODO: Group the Unit Tests into namespaces (and add tests for failure cases)
            var successes = new List<string>();
            var failures = new List<string>();
            foreach (var testItem in allTestItems)
            {
                if (ExecuteTest(testItem.FullName, testItem.DisplayName, testItem.Serialised, document.body))
                {
                    successes.Add(testItem.DisplayName);
                    SetSuccessCount(successes.Count);
                }
                else
                {
                    console.log("FAILED: " + testItem.DisplayName); // TODO: Remove
                    failures.Add(testItem.DisplayName);
                    SetFailureCount(failures.Count);
                }
                await Task.Delay(1); // Give the UI a chance to update if there have been tests that don't complete almost instantly
            }
            SetStatus("Completed");
        }

        private static IEnumerable<TestItem> GetTestsToRun()
        {
            var allTestItems = TestData.GetItems()
                .Select(item => new TestItem { FullName = item.TestItemName, DisplayName = item.TestItemName, Serialised = item.Serialised })
                .ToArray();

            // If the tests all have names that comes from types within a shared namespace then remove that namespace from the names so that the output is less noisy
            if (allTestItems.Length < 2) // We need at least two tests to find a common namespace!
                return allTestItems;

            var testItemsAndNameSegmentsForThem = allTestItems
                .Select(testItem => new { NameSegments = testItem.FullName.Split('.').ToList(), TestItem = testItem })
                .ToList();
            while (testItemsAndNameSegmentsForThem.All(entry => entry.NameSegments.Count > 1) && (testItemsAndNameSegmentsForThem.Select(entry => entry.NameSegments.First()).Distinct().Count() == 1))
            {
                testItemsAndNameSegmentsForThem.ForEach(entry => entry.NameSegments.RemoveAt(0));
            }
            allTestItems = testItemsAndNameSegmentsForThem
                .OrderBy(entry => entry.NameSegments, TestNameSegmentOrderer.Instance)
                .Select(entry => new TestItem { FullName = entry.TestItem.FullName, DisplayName = string.Join(".", entry.NameSegments), Serialised = entry.TestItem.Serialised })
                .ToArray();

            return allTestItems;
        }

        private static bool ExecuteTest(string fullName, string displayName, byte[] serialised, HTMLElement appendResultMessageTo)
        {
            try
            {
                var testItem = TestItemInstanceCreator.GetInstance(fullName);
                var decoder = GetNonGenericDecoder(MsgPack5Decoder.Default, testItem.DeserialiseAs);
                var clone = decoder(serialised);
                if (ObjectComparer.AreEqual(testItem.Value, clone, out var messageIfNotEqual))
                {
                    appendResultMessageTo.appendChild(GetMessage(displayName, isSuccess: true));
                    return true;
                }
                appendResultMessageTo.appendChild(GetMessage(displayName, isSuccess: false, additionalInfo: messageIfNotEqual));
            }
            catch (Exception e)
            {
                appendResultMessageTo.appendChild(GetMessage(displayName, isSuccess: false, additionalInfo: e.Message));
            }
            return false;
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

        private static (HTMLElement ToDisplay, Action<string> SetStatus, Action<int> SetSuccessCount, Action<int> SetFailureCount) GetRunningSummary(int numberOfTests)
        {
            var runningSummary = new HTMLDivElement();
            runningSummary.style.lineHeight = "1.4";
            runningSummary.style.margin = "0.5rem";

            var status = new HTMLDivElement { innerText = "Running.." };
            status.style.fontWeight = "bold";
            runningSummary.appendChild(status);

            var (successProgress, setSuccessCount) = GetProgressLine("Successes");
            runningSummary.appendChild(successProgress);

            var (failureProgress, setFailureCount) = GetProgressLine("Failures");
            runningSummary.appendChild(failureProgress);

            return (
                runningSummary,
                statusText => status.innerText = statusText,
                setSuccessCount,
                setFailureCount
            );

            (HTMLElement Line, Action<int> SetValue) GetProgressLine(string text)
            {
                var progress = new HTMLDivElement();

                var label = new HTMLSpanElement { innerText = text + ": " };
                label.style.display = "inline-block";
                label.style.width = "10rem";
                progress.appendChild(label);

                var countWrapper = new HTMLSpanElement();
                progress.appendChild(countWrapper);

                var count = new HTMLSpanElement { innerText = "0" };
                countWrapper.appendChild(count);
                countWrapper.appendChild(new HTMLSpanElement { innerText = " / " + numberOfTests });
                
                return (progress, value => count.innerText = value.ToString());
            }
        }

        private static HTMLElement GetMessage(string text, bool isSuccess, string additionalInfo = null)
        {
            var wrapper = new HTMLDivElement();
            wrapper.style.color = "white";
            wrapper.style.backgroundColor = isSuccess ? "#0a0" : "#c00";
            wrapper.style.border = "1px solid " + (isSuccess ? "#080" : "#900");
            wrapper.style.borderRadius = "0.25rem";
            wrapper.style.padding = "0.5rem 1rem";
            wrapper.style.marginBottom = "0.5rem";

            var message = new HTMLDivElement { innerText = text };
            wrapper.appendChild(message);

            if (!string.IsNullOrWhiteSpace(additionalInfo))
            {
                var additionalMessage = new HTMLDivElement { innerText = additionalInfo };
                additionalMessage.style.fontSize = "0.8rem";
                wrapper.appendChild(additionalMessage);
            }

            return wrapper;
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

        private sealed class TestItem
        {
            public string FullName { get; set; }
            public string DisplayName { get; set; }
            public byte[] Serialised { get; set; }
        }
    }
}