using System;
using System.Collections.Generic;
using SharedTestItems;

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
                    var clone = MsgPack5Decoder.Default.Decode(serialised, testItem.DeserialiseAs);
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
    }
}