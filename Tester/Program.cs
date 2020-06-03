using System;
using System.Collections.Generic;
using MessagePack;
using MsgPack5.H5;
using MsgPack5.H5.Tests.SharedTypes;
using Newtonsoft.Json;

namespace Tester
{
    class Program
    {
        private static void Main()
        {
            SimpleTests();
            Console.WriteLine();

            InterfaceTests();
            Console.WriteLine();

            Console.WriteLine("Done!");
            Console.WriteLine("Press [Enter] to terminate.");
            Console.ReadLine();
        }

        private static void SimpleTests()
        {
            var sourceItem = new SomethingWithKeyAndID { Key = 123, ID = "TEST" };
            var serialisedItem = MessagePackSerializer.Serialize(sourceItem);
            var cloneItem = MsgPack5Decoder.Default.Decode<SomethingWithKeyAndID>(serialisedItem);

            var sourceDictionary = new Dictionary<string, uint> { { "One", 1 }, { "Two", 2 } };
            var serialisedDictionary = MessagePackSerializer.Serialize(sourceDictionary);
            var cloneDictionary = MsgPack5Decoder.Default.Decode<Dictionary<string, uint>>(serialisedDictionary);

            Console.WriteLine("Successful cloning of:");
            foreach (var clone in new object[] { cloneItem, cloneDictionary })
                Console.WriteLine(ToDetailedJsonRepresentation(clone));
        }

        private static void InterfaceTests()
        {
            var source0 = new Thing0 { Name = "Dan", Age = 123 };
            var source1 = new Thing1 { Roles = new[] { "Tester", "Cat Herder" } };
            var source2 = new Thing2 { Day = DayOfWeek.Tuesday };
            var wrapper = new Wrapper { Things = new IThing[] { source0, source1, source2 } };

            var serialisedConcreteSource0 = MessagePackSerializer.Serialize(source0);
            var serialisedInterfaceSource0 = MessagePackSerializer.Serialize<IThing>(source0);

            var serialisedConcreteSource1 = MessagePackSerializer.Serialize(source1);
            var serialisedInterfaceSource1 = MessagePackSerializer.Serialize<IThing>(source1);

            var serialisedWrapper = MessagePackSerializer.Serialize(wrapper);

            var cloneConcrete0 = MsgPack5Decoder.Default.Decode<Thing0>(serialisedConcreteSource0);
            var cloneInterface0 = MsgPack5Decoder.Default.Decode<IThing>(serialisedInterfaceSource0);

            var cloneConcrete1 = MsgPack5Decoder.Default.Decode<Thing1>(serialisedConcreteSource1);
            var cloneInterface1 = MsgPack5Decoder.Default.Decode<IThing>(serialisedInterfaceSource1);

            var cloneWrapper = MsgPack5Decoder.Default.Decode<Wrapper>(serialisedWrapper);

            Console.WriteLine("Successful cloning of:");
            foreach (var clone in new object[] { cloneConcrete0, cloneInterface0, cloneConcrete1, cloneInterface1, cloneWrapper })
                Console.WriteLine(ToDetailedJsonRepresentation(clone));
        }

        private static string ToDetailedJsonRepresentation(object value) => (value is null) ? "{null}" : JsonConvert.SerializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
    }
}