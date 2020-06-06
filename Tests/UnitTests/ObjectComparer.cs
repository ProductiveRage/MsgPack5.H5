using Newtonsoft.Json;

namespace UnitTests
{
    internal static class ObjectComparer
    {
        public static bool AreEqual(object x, object y)
        {
            // There might be a better way to do this (a testing library for that could be made to work with H5 but this should suffice for now)
            return SerialiseToJson(x) == SerialiseToJson(y);

            string SerialiseToJson(object value) => JsonConvert.SerializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
    }
}