using System;

namespace Some1.User.Unity
{
    public sealed class FpsWatch
    {
        private double _deltaTime;

        public int ComputeFps(double deltaTime)
        {
            //float msec = _deltaTime * 1000.0f;
            //float fps = 1.0f / _deltaTime;
            //text.text = string.Format("{0:0.0} ms{1}{2:0.} fps", msec, Environment.NewLine, fps);

            _deltaTime += (deltaTime - _deltaTime) * 0.1f;
            var fps = 1.0f / _deltaTime;
            return (int)Math.Round(fps);
        }

        public void Reset()
        {
            _deltaTime = default;
        }
    }
}
