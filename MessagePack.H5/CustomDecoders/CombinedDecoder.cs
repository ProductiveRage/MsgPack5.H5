using System;
using System.Collections.Generic;
using System.Linq;

namespace MessagePack
{
    public sealed class CombinedDecoder : ICustomDecoder
    {
        private readonly ICustomDecoder[] _decoders;
        public CombinedDecoder(IEnumerable<ICustomDecoder> decoders)
        {
            if (decoders is null)
                throw new ArgumentNullException(nameof(decoders));

            _decoders = decoders.ToArray();
            if (_decoders.Any(decoder => decoder is null))
                throw new ArgumentException("null reference encountered", nameof(decoders));
        }

        public MsgPack5Decoder.Decoder TryToGetDecoder(sbyte typeCode)
        {
            foreach (var potentialDecoder in _decoders)
            {
                var decoder = potentialDecoder.TryToGetDecoder(typeCode);
                if (decoder is object)
                    return decoder;
            }
            return null;
        }

        public CombinedDecoder Add(ICustomDecoder decoder)
        {
            if (decoder is null)
                throw new ArgumentNullException(nameof(decoder));

            return new CombinedDecoder(_decoders.Concat(new[] { decoder }));
        }
    }
}