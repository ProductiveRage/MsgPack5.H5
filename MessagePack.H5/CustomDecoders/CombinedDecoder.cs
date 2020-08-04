﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MessagePack
{
    public sealed class CombinedDecoder
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

        public MsgPack5Decoder.Decoder GetDecoder(sbyte typeCode)
        {
            foreach (var potentialDecoder in _decoders)
            {
                var decoder = potentialDecoder.TryToGetDecoder(typeCode);
                if (decoder is object)
                    return decoder;
            }
            return null;
        }
    }
}