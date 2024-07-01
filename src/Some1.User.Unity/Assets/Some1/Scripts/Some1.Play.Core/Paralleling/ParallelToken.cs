using System;

namespace Some1.Play.Core.Paralleling
{
    public sealed class ParallelToken
    {
        public ParallelToken(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            Index = index;
        }

        public int Index { get; }

        public int Code { get; private set; }

        public void NewCode()
        {
            if (Code == int.MaxValue)
            {
                Code = 1;
            }
            else
            {
                Code++;
            }
        }
    }
}
