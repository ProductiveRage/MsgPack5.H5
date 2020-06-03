using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MessagePack;
using MsgPack5.Bridge;
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

            Console.WriteLine("Done! Press [Enter] to terminate..");
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

    [MessagePack.MessagePackObject]
    [MsgPack5.Bridge.MessagePackObject]
    public class SomethingWithKeyAndID
    {
        [MessagePack.Key(0)]
        [MsgPack5.Bridge.Key(0)]
        public int Key { get; set; }

        [MessagePack.Key(1)]
        [MsgPack5.Bridge.Key(1)]
        public string ID { get; set; }
    }

    [MessagePack.MessagePackObject]
    [MsgPack5.Bridge.MessagePackObject]
    public sealed class Wrapper
    {
        [MessagePack.Key(0)]
        [MsgPack5.Bridge.Key(0)]
        public IThing[] Things { get; set; }
    }

    [MessagePack.Union(0, typeof(Thing0))]
    [MessagePack.Union(1, typeof(Thing1))]
    [MessagePack.Union(2, typeof(Thing2))]
    [MsgPack5.Bridge.Union(0, typeof(Thing0))]
    [MsgPack5.Bridge.Union(1, typeof(Thing1))]
    [MsgPack5.Bridge.Union(2, typeof(Thing2))]
    public interface IThing { }

    [MessagePack.MessagePackObject]
    [MsgPack5.Bridge.MessagePackObject]
    public sealed class Thing0 : IThing
    {
        [MessagePack.Key(0)]
        [MsgPack5.Bridge.Key(0)]
        public string Name { get; set; }

        [MessagePack.Key(1)]
        [MsgPack5.Bridge.Key(1)]
        public int Age { get; set; }
    }

    [MessagePack.MessagePackObject]
    [MsgPack5.Bridge.MessagePackObject]
    public sealed class Thing1 : IThing
    {
        [MessagePack.Key(0)]
        [MsgPack5.Bridge.Key(0)]
        public string[] Roles { get; set; }

        [MessagePack.Key(1)]
        [MsgPack5.Bridge.Key(1)]
        public int[] IDs { get; set; }
    }

    [MessagePack.MessagePackObject]
    [MsgPack5.Bridge.MessagePackObject]
    public sealed class Thing2 : IThing
    {
        [MessagePack.Key(0)]
        [MsgPack5.Bridge.Key(0)]
        public DayOfWeek Day { get; set; }
    }

    [MessagePack.MessagePackObject]
    [MsgPack5.Bridge.MessagePackObject]
    public sealed class ClassWithDictionary
    {
        [MessagePack.Key(0)]
        [MsgPack5.Bridge.Key(0)]
        public Dictionary<string, int> Info { get; set; }
    }
}