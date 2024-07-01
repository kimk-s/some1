using System;
using System.Numerics;

namespace Some1.User.ViewModel
{
    public enum PlayUiSize
    {
        Big,
        Small,
        Big_Default = Big,
    }

    public static class PlayUiSizeExtensions
    {
        public static Vector2 ToResolution(this PlayUiSize size) => size switch
        {
            PlayUiSize.Big => new(1920, 1080),
            PlayUiSize.Small => new(2560, 1440),
            _ => throw new InvalidOperationException()
        };
    }
}
