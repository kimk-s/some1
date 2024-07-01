using System;
using System.Diagnostics;
using Some1.Play.Core;
using static Some1.Play.Client.IPlayStreamer;

namespace Some1.Play.Client.InMemory
{
    public sealed class InMemoryPlayStreamer : IPlayStreamer
    {
        private readonly IPlayCore _core;
        private GetTimeDelegate _getTime = null!;
        private ReadDelegate _read = null!;
        private GetReadableDeltaTimeDelegate _getReadableDeltaTime = null!;
        private InterpolateDelegate _interpolate = null!;
        private SetLocalTimeScaleDelegate _setLocalTimeScale = null!;

        public InMemoryPlayStreamer(IPlayCore core)
        {
            _core = core;
        }

        private int TimeCombo { get; set; }

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

        public void ResetState()
        {
            TimeCombo = 0;
        }

        public void Update(float localDeltaSeconds)
        {
            if (localDeltaSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(localDeltaSeconds));
            }

            UpdateByFlexableCoreDeltaTime(localDeltaSeconds);
            //UpdateByFixedCoreDeltaTime(localDeltaSeconds);
        }

        private void UpdateByFlexableCoreDeltaTime(float localDeltaSeconds)
        {
            if (TimeCombo == 0)
            {
                TimeCombo++;
            }
            else
            {
                if (localDeltaSeconds == 0)
                {
                    throw new InvalidOperationException();
                }

                _core.Update(localDeltaSeconds);
                bool readed = _read();
                Debug.Assert(readed);
                Debug.Assert(_getReadableDeltaTime(1) == 0);
            }
            _interpolate(1);
            _setLocalTimeScale(1);
        }

        #region FixedCoreDeltaTime
        //private int FPS { get; } = 60;
        //private float TimeX { get; set; }
        //private float TimeA => _getTime().A;
        //private float TimeB => _getTime().B;
        //private float TimeDelta => _getTime().Delta;

        //private void ResetStateFixedCoreDeltaTime()
        //{
        //    TimeCombo = 0;
        //}

        //private void UpdateByFixedCoreDeltaTime(float localDeltaSeconds)
        //{
        //    Read(localDeltaSeconds);
        //    Interpolate();
        //    SetLocalTimeScale();
        //}

        //private void Read(float localDeltaSeconds)
        //{
        //    if (TimeCombo == 0)
        //    {
        //        ReadCore();
        //        TimeX = TimeA;
        //    }
        //    else
        //    {
        //        if (localDeltaSeconds == 0)
        //        {
        //            throw new InvalidOperationException();
        //        }

        //        TimeX += localDeltaSeconds;

        //        while (TimeX >= TimeB)
        //        {
        //            ReadCore();
        //        }
        //    }
        //}

        //private void ReadCore()
        //{
        //    float frameSeconds = 1f / FPS;
        //    _core.Update(frameSeconds);

        //    bool readed = _read();
        //    Debug.Assert(readed);
        //    Debug.Assert(_getReadableCount(1) == 0);

        //    TimeCombo++;
        //}

        //private void Interpolate()
        //{
        //    float t = TimeCombo == 0 ? 1 : (TimeX - TimeA) / TimeDelta;
        //    Debug.Assert(t >= 0 && t <= 1);
        //    _interpolate(t);
        //}

        //private void SetLocalTimeScale()
        //{
        //    _setLocalTimeScale(1);
        //}
        #endregion
    }
}
