using System;
using System.Threading;

namespace Some1.Play.Core.Internal
{
    internal sealed class Control : IControl
    {
        private readonly ITime _time;
        private int _takenFrameCount;
        private bool _isTaken;

        internal Control(ITime time)
        {
            _time = time ?? throw new ArgumentNullException(nameof(time));
        }

        public int TakenFrameCount => _takenFrameCount;

        public bool CanTake => TakenFrameCount != _time.FrameCount;

        public bool IsTaken => _isTaken && !CanTake;

        internal bool TryTake()
        {
            if (!CanTake || Interlocked.Exchange(ref _takenFrameCount, _time.FrameCount) == _time.FrameCount)
            {
                return false;
            }
            _isTaken = true;
            return true;
        }

        internal void Reset()
        {
            _takenFrameCount = 0;
            _isTaken = false;
        }
    }
}
