using System;
using System.Collections.Generic;
using System.Reflection;
using MsgPack5.H5.Tests.SharedTestItems;

namespace MsgPack5.H5.Tests.UnitTests
{
    // TODO: Turn this into a unit test UI project
    internal static class Program
    {
        private static void Main()
        {
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
                        successes.Add(testItemName);
                        continue;
                    }
                }
                catch { }
                failures.Add(testItemName);
            }

            Console.WriteLine("Number of successes: " + successes.Count);
            foreach (var name in successes)
                Console.WriteLine("- " + name);
            
            Console.WriteLine();

            Console.WriteLine("Number of failures: " + failures.Count);
            foreach (var name in failures)
                Console.WriteLine("- " + name);

            Console.WriteLine();
            Console.WriteLine("Press [Enter] to terminate..");
            Console.ReadLine();
        }

        private static Func<byte[], object> GetNonGenericDecoder(MsgPack5Decoder decoder, Type deserialiseAs)
        {
            var unboundGetDecoderMethod = typeof(Program).GetMethod(nameof(GetDecoder), genericParameterCount: 1, BindingFlags.Static | BindingFlags.NonPublic, binder: null, types: new[] { typeof(MsgPack5Decoder) }, modifiers: null );
            if (unboundGetDecoderMethod is null)
                throw new Exception("Internal error while trying to retrieve method to form a non-generic Decode call for unit test - this shouldn't be possible");

            var getDecoderMethod = unboundGetDecoderMethod.MakeGenericMethod(deserialiseAs);
            return (Func<byte[], object>)getDecoderMethod.Invoke(null, new[] { decoder });
        }

        private static Func<byte[], object> GetDecoder<T>(MsgPack5Decoder decoder) => serialised => decoder.Decode<T>(serialised);
    }
}