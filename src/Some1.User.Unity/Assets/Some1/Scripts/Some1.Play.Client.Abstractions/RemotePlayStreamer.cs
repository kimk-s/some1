using System;
using Microsoft.Extensions.Logging;
using static Some1.Play.Client.IPlayStreamer;

namespace Some1.Play.Client
{
    public sealed class RemotePlayStreamer : IPlayStreamer
    {
        private readonly ILogger<RemotePlayStreamer> _logger;
        private readonly RemotePlayStreamerJitterBuffer _jitterBuffer;
        private GetTimeDelegate _getTime = null!;
        private ReadDelegate _read = null!;
        private GetReadableDeltaTimeDelegate _getReadableDeltaTime = null!;
        private InterpolateDelegate _interpolate = null!;
        private SetLocalTimeScaleDelegate _setLocalTimeScale = null!;

        public RemotePlayStreamer(ILogger<RemotePlayStreamer> logger, RemotePlayStreamerJitterBuffer jitterBuffer)
        {
            _logger = logger;
            _jitterBuffer = jitterBuffer;
        }

        private float JitterBuffer => _jitterBuffer.Value;
        private byte TimeState { get; set; }
        private double TimeX { get; set; }
        private double TimeA => _getTime().A;
        private double TimeB => _getTime().B;
        private float TimeDelta => (float)_getTime().Delta;

        public void Setup(
            GetTimeDelegate getTime,
            ReadDelegate read,
            GetReadableDeltaTimeDelegate getReadableDeltaTime,
            InterpolateDelegate interpolate,
            SetLocalTimeScaleDelegate setLocalTimeScale)
        {
            _getTime = getTime;
            _read = read;
            _getReadableDeltaTime = getReadableDeltaTime;
            _interpolate = interpolate;
            _setLocalTimeScale = setLocalTimeScale;
        }

        public void Update(float localDeltaSeconds)
        {
            if (localDeltaSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(localDeltaSeconds));
            }

            Read(localDeltaSeconds);
            Interpolate();
            SetLocalTimeScale();
        }

        public void ResetState()
        {
            TimeState = 0;
            TimeX = 0;
        }

        private void Read(float localDeltaSeconds)
        {
            if (TimeState == 0)
            {
                if (TryRead())
                {
                    TimeX = TimeA;
                }
            }
            else
            {
                if (localDeltaSeconds == 0)
                {
                    _logger.LogWarning($"Invalid localDeltaSeconds: {localDeltaSeconds }");
                    localDeltaSeconds = 0.00001f;
                }

                TimeX += localDeltaSeconds;

                while (TimeX >= TimeB)
                {
                    if (!TryRead())
                    {
                        _logger.LogDebug($"Failed to read.");
                        ResetState();
                        break;
                    }
                }
            }
        }

        private void Interpolate()
        {
            float t = (float)(TimeState == 0 ? 1 : (TimeX - TimeA) / TimeDelta);
            if (t < 0 || t > 1)
            {
                _logger.LogWarning($"Invalid interpolate time: {t}");
                t = Math.Clamp(t, 0, 1);
            }
            _interpolate(t);
        }

        private void SetLocalTimeScale()
        {
            _setLocalTimeScale(CalculateLocalTimeScale());
        }

        private float CalculateLocalTimeScale()
        {
            if (TimeState == 0)
            {
                return 0;
            }

            const int MaxReadableDeltaTime = 1;

            float readableDeltaTime = _getReadableDeltaTime(MaxReadableDeltaTime);

            if (readableDeltaTime < JitterBuffer)
            {
                return TimeState == 1 ? 0.5f : 1;
            }

            if (TimeState == 1)
            {
                TimeState = 2;
            }

            if (readableDeltaTime == JitterBuffer)
            {
                return 1;
            }

            if (readableDeltaTime < MaxReadableDeltaTime)
            {
                return 2;
            }

            return 100;
        }

        private bool TryRead()
        {
            if (!_read())
            {
                return false;
            }

            if (TimeState == 0)
            {
                TimeState = 1;
            }

            return true;
        }
    }
}
