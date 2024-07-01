using System;

namespace Some1.Play.Core.Paralleling
{
    public sealed class ParallelCodeGroup
    {
        private readonly int[] _codes;

        public ParallelCodeGroup(ParallelOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _codes = new int[options.GetSafeCount()];
        }

        public bool TrySet(ParallelToken token)
        {
            if (_codes[token.Index] == token.Code)
            {
                return false;
            }

            _codes[token.Index] = token.Code;
            return true;
        }
    }
}
