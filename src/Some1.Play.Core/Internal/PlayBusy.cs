using System;
using Some1.Play.Core.Options;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayBusy
    {
        private readonly int _maxCount;
        private readonly float _pauseThreshold;
        private readonly float _resumeThreshold;
        private bool _paused;

        internal PlayBusy(int maxCount, PlayBusyOptions options)
        {
            if (maxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount));
            }

            if (options.PauseThreshold <= 0 || options.PauseThreshold > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(options));
            }

            if (options.ResumeThreshold <= 0 || options.ResumeThreshold > 1 || options.ResumeThreshold > options.PauseThreshold)
            {
                throw new ArgumentOutOfRangeException(nameof(options));
            }

            _maxCount = maxCount;
            _pauseThreshold = options.PauseThreshold;
            _resumeThreshold = options.ResumeThreshold;
        }

        internal float Value { get; private set; }

        internal bool IsFull => Value >= 1;

        internal void Update(int currentCount)
        {
            if (currentCount < 0 || currentCount > _maxCount)
            {
                throw new ArgumentOutOfRangeException(nameof(currentCount));
            }

            float value = (float)currentCount / _maxCount;

            if (_paused)
            {
                if (value < _resumeThreshold)
                {
                    _paused = false;
                }
                else
                {
                    value = 1;
                }
            }
            else
            {
                if (value >= _pauseThreshold)
                {
                    _paused = true;
                    value = 1;
                }
            }

            Value = value;
        }
    }
}
