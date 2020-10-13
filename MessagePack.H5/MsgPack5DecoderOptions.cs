using System;

namespace MessagePack
{
    public sealed class MsgPack5DecoderOptions
    {
        public static MsgPack5DecoderOptions Default { get; } = new MsgPack5DecoderOptions(DateTimeDecoder.Instance, enableImplicitCasts: false); // The .NET library includes its custom DateTime decoder by default and does not enable implicit (or any) casts

        private MsgPack5DecoderOptions(ICustomDecoder customDecoder, bool enableImplicitCasts)
        {
            CustomDecoderIfAny = customDecoder;
            EnableImplicitCasts = enableImplicitCasts;
        }

        public ICustomDecoder CustomDecoderIfAny { get; }
        
        public bool EnableImplicitCasts { get; }

        /// <summary>
        /// This will replace any registered custom decoders
        /// </summary>
        public MsgPack5DecoderOptions WithCustomDecoder(ICustomDecoder decoder) => new MsgPack5DecoderOptions(decoder, EnableImplicitCasts);

        /// <summary>
        /// This will add a custom decoders to any that are already registered
        /// </summary>
        public MsgPack5DecoderOptions AddCustomDecoder(ICustomDecoder decoder)
        {
            if (decoder is null)
                throw new ArgumentNullException(nameof(decoder));

            ICustomDecoder newCombinedDecoder;
            if (CustomDecoderIfAny is null)
                newCombinedDecoder = decoder;
            else if (CustomDecoderIfAny is CombinedDecoder combinedDecoder)
                newCombinedDecoder = combinedDecoder.Add(decoder);
            else
                newCombinedDecoder = new CombinedDecoder(new[] { CustomDecoderIfAny, decoder });

            return new MsgPack5DecoderOptions(newCombinedDecoder, EnableImplicitCasts);
        }

        public MsgPack5DecoderOptions WithEnableImplicitCasts(bool enable = true) => new MsgPack5DecoderOptions(CustomDecoderIfAny, enable);
    }
}