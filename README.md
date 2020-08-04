# MsgPack.H5

This is a project intended for when you want to write client-side code in [h5](https://github.com/theolivenbaum/h5) (which is an off-shoot of [Bridge.net](https://bridge.net/) that supports modern .csproj formats and generally makes developments a little smoother and more efficient, with an off-process compiler service to speed up compilation and try to avoid re-doing compilation work that doesn't need re-doing) and you want to send a large amount of data down to the client in a binary format but using a mechanism that allows it to be accessed in a type-safe manner.

The data format is [MessagePack](https://msgpack.org/index.html) but with the additional typing that is available in .NET via the [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) library.

I started this by taking the [decoder.js](https://github.com/mcollina/msgpack5/blob/master/lib/decoder.js) code from the excellent [msgpack5](https://github.com/mcollina/msgpack5) JavaScript library and then fiddling with it until it became valid C#. At that point, I started layering on type-handling such that classes and other types would be correctly populated based upon the serialised content, much like MessagePack-CSharp does in a .NET hosting environment.

Currently, this *only* deals with deserialisation. The msgpack5 library does serialisation as well and I may expand to that one day (so that larger messages can be sent from the server to the client as well as vice versa) but I'm taking things one step at a time! For the use cases that I've envisaged something like this, receiving relatively large amounts of data from the server has been more valuable than sending them back and so deserialisation seemed like a reasonable place to start.

At this point, I have no interest in trying to replicate *everything* that MessagePack-CSharp does - it has *many* options and is a very mature project. In fact, I currently only have designs on supporting integer [Key] attributes (and not supporting `[MessagePackObject(keyAsPropertyName: true)]` at all) and I only expect to have limited (though not completely absent) support for custom decoders (though **DateTime** handling is implemented, to match MessagePack-CSharp's behaviour, via a custom **ICustomDecoder** implementation that is enabled by default but which can be overridden or extended with other custom decoders if required).

The simplest way in is to call one of the static method overloads for

	T MessagePackSerializer.Deserialise<T>(data)
	
.. where data can be a **byte[]** or a **Uint8Array** or a **ArrayBuffer** (so if you're using an xmlhttp instance with a responseType of "arraybuffer" then you can use the response directly with this method).

The intention is to support all of the standard casts that MessagePack-CSharp does, so numbers can be deserialised into byte, int, etc.. (so long as they wouldn't overflow) and complex nested type hierachies and generic types can be successfully populated.

## Parity with the MessagePack-CSharp .NET library

The unit tests are based upon classes shared between both a .NET "UnitTestDataGenerator" project and an h5 "UnitTests" project, so they are serialised using the .NET library and then deserialised again to provide the source data for the tests and then the Unit Tests themselves are executed in an h5 environment and the results - whether they be successful deserialisations or not - replicated precisely.

Currently, there is still a range of unsupported target types (which `T[]` and `IEnumerable<T>` are supported for example, deserialising to `List<T>` is still on the roadmap - but coming soon!)

Suppport for deserialising to abstract types, so long as [Union(subTypeCode, type)] are specified on the base type, is supported and deserialisation via property-setters-only and via constructor (_plus_ property setters, where applicable) is a work in progress but in many cases is operational. This will also improve over time.

Each step in improvement in the deserialisation process brings this me closer to being able to being able to implement a good _serialisation_ process in the future (again, likely starting with the excellent base point of the [encoding in the JS MsgPack5 library](https://github.com/mcollina/msgpack5/blob/master/lib/encoder.js) and then layering in C# type support). But for now, the focus is very much on efficiently dealing with larger binary downloads from the server to the client.

## h5, rather than Bridge.NET?

Where I currently work, we have switched to using h5 over Bridge.NET because it supports the more modern .csproj format, which makes development simpler. I'm hoping that Bridge will take some of the improvements from h5 and absorb them into their code base but it would also be feasible, if it made sense, to have a project that took the h5 code and recompiled using Bridge.NET such that NuGet packages for both compilers would be available.

Right now, there isn't a NuGet package available for either - but one or both are definitely going to be coming soon!