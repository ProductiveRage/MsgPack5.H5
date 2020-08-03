using Newtonsoft.Json;

namespace MessagePack.Tests.SharedTestItems
{
    internal static class JsonSerialiserForComparison
    {
        public static string ToJson(object value)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
#if !H5
            // If the UnitTestDataGenerator needs to include an AlternateResultJson that includes type names where those type names refer to shared types then we don't want to include the assembly name because it won't matched between it ("UnitTestDataGenerator")
            // and where the shared types are built into the "UnitTests" assembly and so JSON deserialisation in the Unit Tests project will fail
            settings.SerializationBinder = TypeNameAssemblyExcludingSerializationBinder.Instance;
#endif
            return JsonConvert.SerializeObject(value, settings);
        }

#if !H5
        private sealed class TypeNameAssemblyExcludingSerializationBinder : Newtonsoft.Json.Serialization.ISerializationBinder
        {
            public static TypeNameAssemblyExcludingSerializationBinder Instance { get; } = new TypeNameAssemblyExcludingSerializationBinder();
            private TypeNameAssemblyExcludingSerializationBinder() { }

            public void BindToName(System.Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;
                typeName = serializedType.FullName;
            }

            public System.Type BindToType(string assemblyName, string typeName) => throw new System.NotImplementedException();
        }
#endif
    }
}