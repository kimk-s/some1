using System;
using System.Linq;
using System.Numerics;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class CastIndicatorViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public CastIndicatorViewModel(
            IPlayerObjectCastFront cast,
            IObjectTransformFront transform,
            ReadOnlyReactiveProperty<CastUiState?> uiState)
        {
            Active = uiState
                .Select(x => x is not null && x.Value.Joystick.IsOn && !x.Value.Joystick.IsMagnitudeInClick)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Position = transform.Position.Connect().AddTo(_disposables);

            var color = uiState
                .CombineLatest(
                    cast.Items[CastId.Attack].AnyLoadCount,
                    cast.Items[CastId.Super].AnyLoadCount,
                    cast.Items[CastId.Ultra].AnyLoadCount,
                    (uiState, a, s, u) =>
                    {
                        if (uiState is null)
                        {
                            return IndicatorColor.White;
                        }

                        bool value = uiState.Value.Id switch
                        {
                            CastId.Attack => a,
                            CastId.Super => s,
                            CastId.Ultra => u,
                            _ => throw new NotImplementedException()
                        };

                        if (!value)
                        {
                            return IndicatorColor.Red;
                        }

                        return uiState.Value.Id == CastId.Attack ? IndicatorColor.White : IndicatorColor.Yellow;
                    })
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Straight = new StraightCastIndicatorViewModel(Active, cast, uiState, color).AddTo(_disposables);

            CircularSector = new CircularSectorCastIndicatorViewModel(Active, cast, uiState, color).AddTo(_disposables);

            Jump = new JumpCastIndicatorViewModel(Active, cast, uiState, color).AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<Vector2> Position { get; }

        public StraightCastIndicatorViewModel Straight { get; }

        public CircularSectorCastIndicatorViewModel CircularSector { get; }

        public JumpCastIndicatorViewModel Jump { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
