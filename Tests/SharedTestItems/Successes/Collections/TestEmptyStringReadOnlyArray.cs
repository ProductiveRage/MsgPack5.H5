﻿using System;

namespace MessagePack.Tests.SharedTestItems.Successes.Collections
{
#if H5 // Have to implement this class differently between .NET and h5 because we want .NET to serialise the data as an array and for the h5 class to deserialise it as a ReadOnlyArray (which is a type that the server is not aware of)
    using UnitTests;
    internal sealed class TestEmptyStringReadOnlyArray : SuccessTestItem<ReadOnlyArray<string>>
    {
        public TestEmptyStringReadOnlyArray() : base(new ReadOnlyArray<string>(Array.Empty<string>())) { }

        public override Func<MsgPack5DecoderOptions, MsgPack5DecoderOptions> DecodeOptions => options => options.WithEnableImplicitCasts(); // We'll need to enable implicit casts (which is non-standard behaviour in the .NET library) to allow returned ReadOnlyArray instances
    }
#else
    internal sealed class TestEmptyStringReadOnlyArray : SuccessTestItem<string[]>
    {
        public TestEmptyStringReadOnlyArray() : base(Array.Empty<string>()) { }
    }
#endif
}