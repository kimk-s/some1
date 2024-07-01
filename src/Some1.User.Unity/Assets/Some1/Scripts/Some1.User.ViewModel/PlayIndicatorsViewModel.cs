using System;
using R3;
using Some1.Play.Front;

namespace Some1.User.ViewModel
{
    public sealed class PlayIndicatorsViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayIndicatorsViewModel(
            IPlayFront front,
            ReadOnlyReactiveProperty<CastUiState?> castUiState,
            ReadOnlyReactiveProperty<JoystickUiState> walkUiState,
            ReadOnlyReactiveProperty<bool> likeUiState)
        {
            CastIndicator = new CastIndicatorViewModel(front.Player.Object.Cast, front.Player.Object.Transform, castUiState).AddTo(_disposables);

            WalkIndicator = new WalkIndicatorViewModel(front.Player.Object, walkUiState).AddTo(_disposables);

            LikeIndicator = new LikeIndicatorViewModel(front.Player.Object.Transform, front.Player.Emojis, likeUiState).AddTo(_disposables);
        }

        public WalkIndicatorViewModel WalkIndicator { get; }

        public CastIndicatorViewModel CastIndicator { get; }

        public LikeIndicatorViewModel LikeIndicator { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
