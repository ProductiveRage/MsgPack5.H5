using System;
using System.Collections.Generic;
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
            // TODO: Group the Unit Tests into namespaces (and add tests for failure cases)
            // TODO: Maybe show failures first? Or at least a summary at the top to indicate the numbers of successes and failures?
            var successes = new List<string>();
            var failures = new List<string>();
            foreach (var (testItemName, serialised) in TestData.GetItems())
            {
                try
                {
                    var testItem = TestItemInstanceCreator.GetInstance(testItemName);
                    var decoder = GetNonGenericDecoder(MsgPack5Decoder.Default, testItem.DeserialiseAs);
                    var clone = decoder(serialised);
                    if (ObjectComparer.AreEqual(testItem.Value, clone))
                    {
                        document.body.appendChild(GetMessage(testItemName, isSuccess: true));
                        successes.Add(testItemName);
                        continue;
                    }
                }
                catch { }
                document.body.appendChild(GetMessage(testItemName, isSuccess: true));
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
    }
}