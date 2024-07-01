using System;

namespace Some1.Wait.Front.Internal
{
    internal sealed class WaitPlayChannelFront : IWaitPlayChannelFront
    {
        public WaitPlayChannelFront(int number, string playId, bool openingSoon, bool maintenance, float busy, bool isFull)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            Number = number;
            PlayId = playId;
            OpeningSoon = openingSoon;
            Maintenance = maintenance;
            Busy = busy;
            IsFull = isFull;
        }

        public int Number { get; }

        public string PlayId { get; }

        public bool OpeningSoon { get; }

        public bool Maintenance { get; }

        public float Busy { get; }

        public bool IsFull { get; }
    }
}
