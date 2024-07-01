using System;
using System.Numerics;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayCameraViewModel : IDisposable
    {
        public PlayCameraViewModel(IPlayerFront front)
        {
            Position = front.Object.Transform.Position;
        }

        public ReadOnlyReactiveProperty<Vector2> Position { get; }

        public void Dispose()
        {
        }
    }
}
