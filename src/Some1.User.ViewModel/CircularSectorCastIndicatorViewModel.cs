using System;
using System.Numerics;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class CircularSectorCastIndicatorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public CircularSectorCastIndicatorViewModel(
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

                    return info.Aim.Type == CastAimType.CircularSector;
                })
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            HandleRadiusAndAngle = Active
                .CombineLatest(
                    uiState,
                    (active, uiState) =>
                    {
                        if (!active || uiState is null)
                        {
                            return Vector2.Zero;
                        }

                        var info = front.Items[uiState.Value.Id].Info.CurrentValue;
                        if (info is null)
                        {
                            return Vector2.Zero;
                        }

                        return new(info.Aim.Length, info.Aim.Angle);
                    })
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            HandleOffset = Active
                .CombineLatest(
                    uiState,
                    (active, uiState) =>
                    {
                        if (!active || uiState is null)
                        {
                            return 0;
                        }

                        var info = front.Items[uiState.Value.Id].Info.CurrentValue;
                        if (info is null)
                        {
                            return 0;
                        }

                        return info.Aim.Offset;
                    })
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            HandleRotation = Active
                .CombineLatest(uiState, (active, uiState) => !active || uiState is null ? 0 : uiState.Value.Joystick.Rotation)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Color = color;
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<Vector2> HandleRadiusAndAngle { get; }

        public ReadOnlyReactiveProperty<float> HandleOffset { get; }

        public ReadOnlyReactiveProperty<float> HandleRotation { get; }

        public ReadOnlyReactiveProperty<IndicatorColor> Color { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
