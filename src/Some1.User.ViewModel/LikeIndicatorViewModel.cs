using System;
using Some1.Play.Front;
using R3;
using System.Numerics;

namespace Some1.User.ViewModel
{
    public sealed class LikeIndicatorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public LikeIndicatorViewModel(IObjectTransformFront transform, IPlayerEmojiGroupFront emojis, ReadOnlyReactiveProperty<bool> uiState)
        {
            Active = emojis.LikeDelay
                .CombineLatest(
                    uiState,
                    (a, b) => a <= 0 && b)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Position = transform.Position.Connect().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<Vector2> Position { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
