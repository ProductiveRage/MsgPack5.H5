using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Tests.SharedTestItems;
using Newtonsoft.Json;
using static H5.Core.dom;
using static H5.Core.es5;

namespace UnitTests
{
    // Note: This project has a PreBuild step that will run the .NET UnitTestDataGenerator project and so ensure that any changes to test items in the SharedTestItems project are reflected in the generated TestData.cs file before this project builds
    internal static class Program
    {
        private const string _testFilterQueryStringName = "test";

        private static async Task Main()
        {
            var allTestItemDetails = GetTestsToRun().ToArray();
            if (!allTestItemDetails.Any())
            {
                document.body.appendChild(GetMessage("There were no tests found to run", hrefIfTextShouldLink: null, isSuccess: false));
                return;
            }

            var testNamesToFilterTo = new HashSet<string>(GetAnyTestsSpecifiedInQueryString()); // If this is empty then ALL tests will be run

            var (summaryContentToDisplay, setStatus, setSuccessCount, setFailureCount, setSkippedCount) = GetRunningSummary(allTestItemDetails.Length, showSkippedCount: testNamesToFilterTo.Any()); // Only show the number of skipped tests if not running them all
            document.body.appendChild(summaryContentToDisplay);

            var failureContainer = document.createElement("div");
            document.body.appendChild(failureContainer);

            var successContainer = document.createElement("div");
            document.body.appendChild(successContainer);

            var skipped = new List<TestDetails>();
            var successes = new List<TestDetails>();
            var failures = new List<TestDetails>();
            foreach (var testItemDetails in allTestItemDetails)
            {
                if (testNamesToFilterTo.Any() && !testNamesToFilterTo.Contains(testItemDetails.FullName) && !testNamesToFilterTo.Contains(testItemDetails.DisplayName))
                {
                    skipped.Add(testItemDetails);
                    setSkippedCount(skipped.Count);
                }
                else if (ExecuteTest(testItemDetails, showDetailedInformation: testNamesToFilterTo.Any(), successContainer, failureContainer))
                {
                    successes.Add(testItemDetails);
                    setSuccessCount(successes.Count);
                }
                else
                {
                    failures.Add(testItemDetails);
                    setFailureCount(failures.Count);
                }
                await Task.Delay(1); // Give the UI a chance to update if there have been tests that don't complete almost instantly
            }
            setStatus("Completed");

            if (testNamesToFilterTo.Any())
            {
                var runAllTestsLinks = new HTMLAnchorElement { href = GetHrefForDisablingFiltering() };
                runAllTestsLinks.innerText = "Return to running all tests";
                runAllTestsLinks.style.padding = "0 0.5rem";
                runAllTestsLinks.style.color = "black";
                document.body.appendChild(runAllTestsLinks);
            }
        }

        private static IEnumerable<TestDetails> GetTestsToRun()
        {
            var allTestItems = TestData.GetItems()
                .Select(item => new TestDetails(item.TestItemName, item.TestItemName, item.Serialised, item.AlternateResultJson, item.ExpectedError))
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

        private static bool ExecuteTest(TestDetails testItemDetails, bool showDetailedInformation, HTMLElement appendSuccessesTo, HTMLElement appendFailuresTo)
        {
            try
            {
                var testItem = TestItemInstanceCreator.GetInstance(testItemDetails.FullName);
                var decoder = GetTypedMessagePackSerializerDeserializeCall(testItem.DeserialiseAs);
                if (testItemDetails.ExpectedError is null)
                {
                    var clone = decoder(testItemDetails.Serialised);
                    var expectedResult = (testItemDetails.AlternateResultJson is null) ? testItem.Value : DeserialiseAlternateResultJson(testItemDetails.AlternateResultJson);
                    if (ObjectComparer.AreEqual(expectedResult, clone, out var messageIfNotEqual))
                    {
                        RenderSuccess($"Expected and received: {JsonSerialiserForComparison.ToJson(expectedResult)}");
                        return true;
                    }
                    RenderFailure(messageIfNotEqual);
                    return false;
                }
                else
                {
                    try
                    {
                        decoder(testItemDetails.Serialised);
                        RenderFailure("No exception was thrown but expected: " + testItemDetails.ExpectedError.ExceptionType.FullName);
                        return false;
                    }
                    catch (Exception deserialisationException)
                    {
                        if (deserialisationException.GetType() != testItemDetails.ExpectedError.ExceptionType)
                        {
                            var additionalInfo = $"Expected exception {testItemDetails.ExpectedError.ExceptionType.FullName} but {deserialisationException.GetType().FullName} was thrown";
                            if (showDetailedInformation)
                                additionalInfo += "\n\n" + deserialisationException.ToString();
                            RenderFailure(additionalInfo);
                            return false;
                        }
                        if (deserialisationException.Message != testItemDetails.ExpectedError.Message)
                        {
                            var additionalInfo = $"Expected exception message \"{testItemDetails.ExpectedError.Message}\" but received \"{deserialisationException.Message}\"";
                            if (showDetailedInformation)
                                additionalInfo += "\n\n" + deserialisationException.ToString();
                            RenderFailure(additionalInfo);
                            return false;
                        }
                        RenderSuccess($"Expected and received error: {deserialisationException.Message}");
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                RenderFailure(e.Message, e.ToString());
            }
            return false;

            void RenderSuccess(string extendedAdditionalInfo)
            {
                appendSuccessesTo.appendChild(
                    GetMessage(testItemDetails.DisplayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(testItemDetails.DisplayName), isSuccess: true, additionalInfo: showDetailedInformation ? extendedAdditionalInfo : null)
                );
            }

            void RenderFailure(string summary, string extendedAdditionalInfo = null)
            {
                appendFailuresTo.appendChild(
                    GetMessage(testItemDetails.DisplayName, hrefIfTextShouldLink: GetHrefForFilteringToTest(testItemDetails.DisplayName), isSuccess: false, additionalInfo: showDetailedInformation ? (extendedAdditionalInfo ?? summary) : summary)
                );
            }
        }

        /// <summary>
        /// There is a bug with the h5/Bridge.NET version of Json.NET (https://github.com/bridgedotnet/Bridge.Newtonsoft.Json/issues/168) where $type string isn't always used in the deserialisation of the top level type - if there is a $type
        /// string there then it should be possible to call DeserialiseObject with a generic type parameter of simply object and for it to read the $type and use the remaining data in the JSON to populate an object of that type but it doesn't
        /// work - for example, if the JSON describes a List of int with three values and the correct $type string is present then a List of int will be returned but it will be empty. This method offers a workaround to that, where it will
        /// do a cheap investigation of the data to see if there is a top-level $type string and then resolve that to a type and deserialise as that type (otherwise it will default to deserialising to type object).
        /// </summary>
        private static object DeserialiseAlternateResultJson(string json)
        {
            var rawResult = H5.Script.Write<dynamic>("JSON.parse({0})", json);
            if (rawResult is null)
                return rawResult;
            
            var typeName = (string)rawResult["$type"];
            var type = string.IsNullOrWhiteSpace(typeName)
                ? typeof(object)
                : Type.GetType(typeName);

            return JsonConvert.DeserializeObject(json, type);
        }

        private static string GetHrefForFilteringToTest(string displayName) => $"?{_testFilterQueryStringName}={encodeURIComponent(displayName)}";
        private static string GetHrefForDisablingFiltering() => window.location.href.Split('?')[0];

        private static Func<byte[], object> GetTypedMessagePackSerializerDeserializeCall(Type deserialiseAs)
        {
            var unboundDeserializeMethod = typeof(MessagePackSerializer).GetMethod(nameof(MessagePackSerializer.Deserialize), BindingFlags.Static | BindingFlags.Public, parameterTypes: new[] { typeof(byte[]) });
            if (unboundDeserializeMethod is null)
                throw new Exception("Internal error while trying to retrieve method to form a non-generic MessagePackSerializer.Deserialize call for unit test - this shouldn't be possible");

            var deserializeMethod = unboundDeserializeMethod.MakeGenericMethod(deserialiseAs);
            return serialised =>
            {
                return deserializeMethod.Invoke(null, serialised);
            };
        }

        private static (HTMLElement SummaryContentToDisplay, Action<string> SetStatus, Action<int> SetSuccessCount, Action<int> SetFailureCount, Action<int> SetSkippedCount) GetRunningSummary(int numberOfTests, bool showSkippedCount)
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
            if (showSkippedCount)
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

        private sealed class TestDetails
        {
            public TestDetails(string fullName, string displayName, byte[] serialised, string alternateResultJson, ExceptionSummary expectedError)
            {
                FullName = !string.IsNullOrWhiteSpace(fullName) ? fullName : throw new ArgumentException("may not be null, blank or whitespace-only", nameof(fullName));
                DisplayName = !string.IsNullOrWhiteSpace(displayName) ? displayName : throw new ArgumentException("may not be null, blank or whitespace-only", nameof(displayName));
                Serialised = serialised ?? throw new ArgumentNullException(nameof(serialised));
                AlternateResultJson = alternateResultJson;
                ExpectedError = expectedError;
            }

            public string FullName { get; set; }
            public string DisplayName { get; set; }
            public byte[] Serialised { get; set; }
            
            /// <summary>
            /// This will be non-null if the deserialised value should not match the source value of the test item - this is unusual, normally the deserialised version will match the source value (or an exception will be thrown)
            /// </summary>
            public string AlternateResultJson { get; set; }
            
            /// <summary>
            /// This will be non-null if an exception should be thrown when an attempt is made to deserialise the data as the target type
            /// </summary>
            public ExceptionSummary ExpectedError { get; set; }

            public TestDetails WithDisplayName(string displayName) => new TestDetails(FullName, displayName, Serialised, AlternateResultJson, ExpectedError);
        }
    }
}