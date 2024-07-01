using System;

namespace Some1.User.ViewModel
{
    public enum PlayClientFps
    {
        _30,
        _60,
        _120,
        _240,
        _60_Default = _60,
    }

    public static class PlayClientFpsExtensions
    {
        public static int ToFps(this PlayClientFps fps) => fps switch
        {
            PlayClientFps._30 => 30,
            PlayClientFps._60 => 60,
            PlayClientFps._120 => 120,
            PlayClientFps._240 => 240,
            _ => throw new InvalidOperationException()
        };
    }
}
