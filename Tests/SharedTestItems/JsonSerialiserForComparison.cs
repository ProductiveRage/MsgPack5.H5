using Newtonsoft.Json;

namespace MessagePack.Tests.SharedTestItems
{
    internal static class JsonSerialiserForComparison
    {
        public static string ToJson(object value) => JsonConvert.SerializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
    }
}