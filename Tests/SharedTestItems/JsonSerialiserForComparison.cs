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
                // Note: Setting the assemblyName to null here will only remove it from the main type itself - it won't remove it from any types specified as generic type parameters (that's what RemoveAssemblyNames is needed for)
                assemblyName = null;
                typeName = RemoveAssemblyNames(serializedType.FullName);
            }

            public System.Type BindToType(string assemblyName, string typeName) => throw new System.NotImplementedException();

            private static string RemoveAssemblyNames(string typeName)
            {
                var index = 0;
                var content = new System.Text.StringBuilder();
                RecusivelyRemoveAssemblyNames();
                return content.ToString();

                void RecusivelyRemoveAssemblyNames()
                {
                    // If we started inside a type name - eg.
                    //
                    //   "System.Int32, System.Private.CoreLib"
                    //
                    // .. then we want to look for the comma that separates the type name from the assembly information and ignore that content. If we started inside
                    // nested generic type content - eg.
                    //
                    //  "[System.Int32, System.Private.CoreLib], [System.String, System.Private.CoreLib]"
                    //
                    // .. then we do NOT want to start ignoring content after any commas encountered. So it's important to know here which case we're in.
                    var insideTypeName = typeName[index] != '[';

                    var ignoreContent = false;
                    while (index < typeName.Length)
                    {
                        var c = typeName[index];
                        index++;

                        if (insideTypeName && (c == ','))
                        {
                            ignoreContent = true;
                            continue;
                        }

                        if (!ignoreContent)
                            content.Append(c);

                        if (c == '[')
                            RecusivelyRemoveAssemblyNames();
                        else if (c == ']')
                        {
                            if (ignoreContent)
                            {
                                // If we encountered a comma that indicated that we were about to start an assembly name then we'll have stopped adding content
                                // to the string builder but we don't want to lose this closing brace, so explicitly add it in if that's the case
                                content.Append(c);
                            }
                            break;
                        }
                    }
                }
            }
        }
#endif
    }
}