using System;
using System.Numerics;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class JumpCastIndicatorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public JumpCastIndicatorViewModel(
            ReadOnlyReactiveProperty<bool> active,
            IPlayerObjectCastFront front,
            ReadOnlyReactiveProperty<CastUiState?> uiState,
            ReadOnlyReactiveProperty<IndicatorColor> color)
        {
            Active = active
                .CombineLatest(uiState, (active, uiState) =>
                {
                    if (!active || uiState is null)
                    {
                        return false;
                    }

                    var info = front.Items[uiState.Value.Id].Info.CurrentValue;
                    if (info is null)
                    {
                        return false;
                    }

                    return info.Aim.Type == CastAimType.Jump;
                })
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            HandleArea = Active
                .CombineLatest(
                    uiState,
                    (active, uiState) =>
                    {
                        if (!active || uiState is null)
                        {
                            return AreaInfo.Empty;
                        }

                        var info = front.Items[uiState.Value.Id].Info.CurrentValue;
                        if (info is null)
                        {
                            return AreaInfo.Empty;
                        }

                        return info.Aim.Area;
                    })
                .ToReadOnlyReactiveProperty(AreaInfo.Empty)
                .AddTo(_disposables);

            HandlePosition = Active
                .CombineLatest(
                    uiState,
                    (active, uiState) => !active || uiState is null
                        ? Vector2.Zero
                        : Vector2Helper.Normalize(uiState.Value.Joystick.Rotation) * uiState.Value.Joystick.Magnitude)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Color = color;
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<AreaInfo> HandleArea { get; }

        public ReadOnlyReactiveProperty<Vector2> HandlePosition { get; }

        public ReadOnlyReactiveProperty<IndicatorColor> Color { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
