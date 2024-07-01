using System;
using System.Linq;
using System.Numerics;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class WalkIndicatorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public WalkIndicatorViewModel(IPlayerObjectFront obj, ReadOnlyReactiveProperty<JoystickUiState> uiState)
        {
            Active = uiState.Select(x => x.IsOn).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Position = obj.Transform.Position.Connect().AddTo(_disposables);

            HandlePosition = uiState.Select(x => Vector2Helper.Normalize(x.Rotation) * x.Magnitude)
                .ToReadOnlyReactiveProperty().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<Vector2> Position { get; }

        public ReadOnlyReactiveProperty<Vector2> HandlePosition { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
