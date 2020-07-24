using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Tests.SharedTestItems;
using static H5.Core.dom;
using static H5.Core.es5;

namespace UnitTests
{
    internal static class Program
    {
        private const string _testFilterQueryStringName = "test";

        private static async Task Main()
        {
            var allTestItems = GetTestsToRun().ToArray();
            if (!allTestItems.Any())
            {
                document.body.appendChild(GetMessage("There were no tests found to run", hrefIfTextShouldLink: null, isSuccess: false));
                return;
            }

            var testNamesToFilterTo = new HashSet<string>(GetAnyTestsSpecifiedInQueryString()); // If this is empty then ALL tests will be run

            var (ToDisplay, SetStatus, SetSuccessCount, SetFailureCount, SetSkippedCount) = GetRunningSummary(allTestItems.Length, showSkippedCount: testNamesToFilterTo.Any());
            document.body.appendChild(ToDisplay);

            // TODO: Group the Unit Tests into namespaces
            var skipped = new List<TestItem>();
            var successes = new List<TestItem>();
            var failures = new List<TestItem>();
            foreach (var testItem in allTestItems)
            {
                if (testNamesToFilterTo.Any() && !testNamesToFilterTo.Contains(testItem.FullName) && !testNamesToFilterTo.Contains(testItem.DisplayName))
                {
                    skipped.Add(testItem);
                    SetSkippedCount(successes.Count);
                }
                else if (ExecuteTest(testItem.FullName, testItem.DisplayName, testItem.Serialised, testItem.ExpectedError, showFullErrorStackTraceForFailures: testNamesToFilterTo.Any(), document.body))
                {
                    successes.Add(testItem);
                    SetSuccessCount(successes.Count);
                }
                else
                {
                    failures.Add(testItem);
                    SetFailureCount(failures.Count);
                }
                await Task.Delay(1); // Give the UI a chance to update if there have been tests that don't complete almost instantly
            }
            SetStatus("Completed");
        }

        private static IEnumerable<TestItem> GetTestsToRun()
        {
            var allTestItems = TestData.GetItems()
                .Select(item => new TestItem(item.TestItemName, item.TestItemName, item.Serialised, item.ExpectedError))
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
                .Select(entry => entry.TestItem.WithDisplayName(string.Join(".", entry.NameSegments)))
                .ToArray();

            return allTestItems;
        }

        private static bool ExecuteTest(string fullName, string displayName, byte[] serialised, ExceptionSummary expectedError, bool showFullErrorStackTraceForFailures, HTMLElement appendResultMessageTo)
        {
            try
            {
                var testItem = TestItemInstanceCreator.GetInstance(fullName);
                var decoder = GetNonGenericDecoder(MsgPack5Decoder.Default, testItem.DeserialiseAs);
                if (expectedError is null)
                {
                    var clone = decoder(serialised);
                    if (ObjectComparer.AreEqual(testItem.Value, clone, out var messageIfNotEqual))
                    {
                        appendResultMessageTo.appendChild(GetMessage(displayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(displayName), isSuccess: true));
                        return true;
                    }
                    appendResultMessageTo.appendChild(GetMessage(displayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(displayName), isSuccess: false, additionalInfo: messageIfNotEqual));
                }
                else
                {
                    try
                    {
                        decoder(serialised);
                        appendResultMessageTo.appendChild(GetMessage(displayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(displayName), isSuccess: false, additionalInfo: "No exception was thrown but expected: " + expectedError.ExceptionType.FullName));
                        return false;
                    }
                    catch (Exception deserialisationException)
                    {
                        if (deserialisationException.GetType() != expectedError.ExceptionType)
                        {
                            appendResultMessageTo.appendChild(GetMessage(displayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(displayName), isSuccess: false, additionalInfo: $"Expected exception {expectedError.ExceptionType.FullName} but {deserialisationException.GetType().FullName} was thrown"));
                            return false;
                        }
                        if (deserialisationException.Message != expectedError.Message)
                        {
                            appendResultMessageTo.appendChild(GetMessage(displayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(displayName), isSuccess: false, additionalInfo: $"Expected exception message \"{expectedError.Message}\" but received \"{deserialisationException.Message}\""));
                            return false;
                        }
                        appendResultMessageTo.appendChild(GetMessage(displayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(displayName), isSuccess: true));
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                appendResultMessageTo.appendChild(GetMessage(displayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(displayName), isSuccess: false, additionalInfo: showFullErrorStackTraceForFailures ? e.ToString() : e.Message));
            }
            return false;
        }
        
        // TODO: Need a way to remove filtering (..other than just clicking back after navigating to a filtered view?)

        private static string GetHrefForFilteringToTest(string displayName) => $"?{_testFilterQueryStringName}={encodeURIComponent(displayName)}";

        private static Func<byte[], object> GetNonGenericDecoder(MsgPack5Decoder decoder, Type deserialiseAs)
        {
            var unboundGetDecoderMethod = typeof(Program).GetMethod(nameof(GetDecoder), BindingFlags.Static | BindingFlags.NonPublic, parameterTypes: new[] { typeof(MsgPack5Decoder) });
            if (unboundGetDecoderMethod is null)
                throw new Exception("Internal error while trying to retrieve method to form a non-generic Decode call for unit test - this shouldn't be possible");

            var getDecoderMethod = unboundGetDecoderMethod.MakeGenericMethod(deserialiseAs);
            return (Func<byte[], object>)getDecoderMethod.Invoke(null, new[] { decoder });
        }

        private static Func<byte[], object> GetDecoder<T>(MsgPack5Decoder decoder) => serialised => decoder.Decode<T>(serialised);

        private static (HTMLElement ToDisplay, Action<string> SetStatus, Action<int> SetSuccessCount, Action<int> SetFailureCount, Action<int> SetSkippedCount) GetRunningSummary(int numberOfTests, bool showSkippedCount)
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

            var (skippedProgress, setSkippedCount) = GetProgressLine("Skipped");
            runningSummary.appendChild(skippedProgress);

            return (
                runningSummary,
                statusText => status.innerText = statusText,
                setSuccessCount,
                setFailureCount,
                setSkippedCount
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

        private static HTMLElement GetMessage(string text, string hrefIfTextShouldLink, bool isSuccess, string additionalInfo = null)
        {
            var wrapper = new HTMLDivElement();
            wrapper.style.color = "white";
            wrapper.style.backgroundColor = isSuccess ? "#0a0" : "#c00";
            wrapper.style.border = "1px solid " + (isSuccess ? "#080" : "#900");
            wrapper.style.borderRadius = "0.25rem";
            wrapper.style.padding = "0.5rem 1rem";
            wrapper.style.marginBottom = "0.5rem";

            var messageContainer = new HTMLDivElement();
            wrapper.appendChild(messageContainer);

            HTMLElement message;
            if (string.IsNullOrWhiteSpace(hrefIfTextShouldLink))
                message = new HTMLSpanElement();
            else
            {
                message = new HTMLAnchorElement { href = hrefIfTextShouldLink };
                message.style.color = wrapper.style.color;
                message.style.textDecoration = "none";
            }
            message.innerText = text;
            messageContainer.appendChild(message);

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

        private static IEnumerable<string> GetAnyTestsSpecifiedInQueryString()
        {
            var queryString = document.location.search;
            if (string.IsNullOrWhiteSpace(queryString))
                yield break;

            const string startsWith = _testFilterQueryStringName + "=";
            foreach (var entry in queryString.TrimStart('?').Split('&'))
            {
                if (!entry.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase))
                    continue;

                foreach (var value in entry.Substring(startsWith.Length).Split(','))
                {
                    var unencodedValue = decodeURIComponent(value).Trim();
                    if (!string.IsNullOrEmpty(unencodedValue))
                        yield return unencodedValue;
                }
            }
        }

        private sealed class TestItem
        {
            public TestItem(string fullName, string displayName, byte[] serialised, ExceptionSummary expectedError)
            {
                FullName = !string.IsNullOrWhiteSpace(fullName) ? fullName : throw new ArgumentException("may not be null, blank or whitespace-only", nameof(fullName));
                DisplayName = !string.IsNullOrWhiteSpace(displayName) ? displayName : throw new ArgumentException("may not be null, blank or whitespace-only", nameof(displayName));
                Serialised = serialised ?? throw new ArgumentNullException(nameof(serialised));
                ExpectedError = expectedError;
            }

            public string FullName { get; set; }
            public string DisplayName { get; set; }
            public byte[] Serialised { get; set; }
            public ExceptionSummary ExpectedError { get; set; }

            public TestItem WithDisplayName(string displayName) => new TestItem(FullName, displayName, Serialised, ExpectedError);
        }
    }
}