using System;
using System.Collections.Generic;
using System.Linq;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayJoysticksViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayJoysticksViewModel(IPlayFront front)
        {
            var castUiState = new ReactiveProperty<CastUiState?>().AddTo(_disposables);
            CastUiState = castUiState;
            var castCommand = new ReactiveCommand<CastCommand>().AddTo(_disposables);
            short manualToken = 0;
            castCommand.Subscribe(x =>
            {
                front.Cast(new(x.Id, IssueManualToken(ref manualToken), x.IsOn, x.Aim));

                static short IssueManualToken(ref short token)
                {
                    if (token < PlayConst.MinCastManualToken || token >= PlayConst.MaxCastManualToken)
                    {
                        token = PlayConst.MinCastManualToken;
                    }
                    else
                    {
                        token++;
                    }
                    return token;
                }
            });

            Active = front.Player.Object.Alive.Alive
                .Connect()
                .AddTo(_disposables);

            CastJoysticksActive = front.Player.Object.Battle.Battle
                .Select(x => x == true)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);
            CastJoysticksActive
                .Where(x => !x)
                .Subscribe(_ => castUiState.Value = null);

            CastJoysticks = EnumForUnity.GetValues<CastId>()
                .ToDictionary(
                    x => x,
                    x => new CastJoystickViewModel(
                        front.Player.Object.Cast.Items[x],
                        front.Player.Object.Trait,
                        castUiState,
                        castCommand)
                .AddTo(_disposables));

            WalkJoystick = new ReactiveCommand<JoystickUiState>().AddTo(_disposables);
            WalkUiState = WalkJoystick.ToReadOnlyReactiveProperty();
            var previousIsOn = false;
            WalkJoystick.Subscribe(x =>
            {
                if (!x.IsMagnitudeInClick)
                {
                    if (x.IsOn)
                    {
                        front.Walk(new(true, x.Rotation));
                    }
                    else
                    {
                        front.Walk(new(false, x.Rotation));
                    }
                    previousIsOn = x.IsOn;
                }
                else
                {
                    if (previousIsOn)
                    {
                        front.Walk(new(false, x.Rotation));
                        previousIsOn = false;
                    }
                }
            });
        }

        public ReadOnlyReactiveProperty<CastUiState?> CastUiState { get; }

        public ReadOnlyReactiveProperty<JoystickUiState> WalkUiState { get; }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<bool> CastJoysticksActive { get; }

        public IReadOnlyDictionary<CastId, CastJoystickViewModel> CastJoysticks { get; }

        public ReactiveCommand<JoystickUiState> WalkJoystick { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
