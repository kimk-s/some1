using System;
using System.Collections.Generic;

namespace Some1.Play.Core.Paralleling
{
    public sealed class ParallelTokenGroup
    {
        private readonly ParallelToken[] _all;

        public ParallelTokenGroup(ParallelOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _all = new ParallelToken[options.GetSafeCount()];
            for (var i = 0; i < _all.Length; i++)
            {
                _all[i] = new(i);
            }
        }

        public IReadOnlyList<ParallelToken> All => _all;
    }
}
