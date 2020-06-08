using Newtonsoft.Json;

namespace UnitTests
{
    internal static class ObjectComparer
    {
        public static bool AreEqual(object x, object y, out string messageIfNot)
        {
            // There might be a better way to do this (a testing library for that could be made to work with H5 but this should suffice for now)
            var jsonX = SerialiseToJson(x);
            var jsonY = SerialiseToJson(y);
            if (jsonX == jsonY)
            {
                messageIfNot = null;
                return true;
            }

            messageIfNot = $"Expected {jsonX} but received {jsonY}";
            return false;

            string SerialiseToJson(object value) => JsonConvert.SerializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
    }
}