using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class CastJoystickViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly ReactiveCommand<JoystickUiState> _execute = new();

        public CastJoystickViewModel(
            IPlayerObjectCastItemFront front,
            IPlayerObjectTraitFront traitFront,
            ReactiveProperty<CastUiState?> castUiState,
            ReactiveCommand<CastCommand> castCommand)
        {
            Id = front.Id;

            Info = front.Info
                .Select(x => x is not null)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            IsAvailable = castUiState
                .Select(x => x is null || x.Value.Id == front.Id || !x.Value.Joystick.IsOn)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            IsAvailable
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    if (castUiState.Value is not null && castUiState.Value.Value.Id == front.Id)
                    {
                        castUiState.Value = null;
                    }
                });

            Delay = front.Delay.Connect().AddTo(_disposables);

            AnyLoadCount = front.AnyLoadCount.Connect().AddTo(_disposables);

            NextTrait = traitFront.Next.Connect().AddTo(_disposables);

            AfterNextTrait = traitFront.AfterNext.Connect().AddTo(_disposables);

            _execute.AddTo(_disposables);
            _execute.Subscribe(x =>
            {
                if (front.Info.CurrentValue is null || !IsAvailable.CurrentValue)
                {
                    return;
                }

                x = new(
                    x.Type,
                    x.Rotation,
                    Math.Clamp(x.Magnitude * 5, front.Info.CurrentValue.AimLimit.MinLength, front.Info.CurrentValue.AimLimit.MaxLength),
                    x.IsMagnitudeInClick);

                castUiState.Value = new(front.Id, x);

                if (front.Info.CurrentValue.UseOn)
                {
                    switch (x.Type)
                    {
                        case JoystickUiStateType.Down:
                            {
                                castCommand.Execute(new(front.Id, true, Aim.Auto));
                                break;
                            }
                        case JoystickUiStateType.Drag:
                            {
                                castCommand.Execute(new(front.Id, true, new(x.Rotation, x.Magnitude)));
                                break;
                            }
                        case JoystickUiStateType.Up:
                            {
                                if (!x.IsMagnitudeInClick)
                                {
                                    castCommand.Execute(new(front.Id, false, new(x.Rotation, x.Magnitude)));
                                }
                                else
                                {
                                    castCommand.Execute(new(front.Id, false, Aim.Auto));
                                }
                                break;
                            }
                        case JoystickUiStateType.Click:
                            {
                                castCommand.Execute(new(front.Id, false, Aim.Auto));
                                break;
                            }
                        case JoystickUiStateType.Cancel:
                            {
                                castCommand.Execute(new(front.Id, true, Aim.Auto));
                                break;
                            }
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    switch (x.Type)
                    {
                        case JoystickUiStateType.Down:
                            break;
                        case JoystickUiStateType.Drag:
                            break;
                        case JoystickUiStateType.Up:
                            {
                                if (!x.IsMagnitudeInClick)
                                {
                                    castCommand.Execute(new(front.Id, false, new(x.Rotation, x.Magnitude)));
                                }
                                break;
                            }
                        case JoystickUiStateType.Click:
                            {
                                castCommand.Execute(new(front.Id, false, Aim.Auto));
                                break;
                            }
                        case JoystickUiStateType.Cancel:
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            });
        }

        public CastId Id { get; }

        public ReadOnlyReactiveProperty<bool> Info { get; }

        public ReadOnlyReactiveProperty<bool> IsAvailable { get; }

        public ReadOnlyReactiveProperty<float> Delay { get; }

        public ReadOnlyReactiveProperty<bool> AnyLoadCount { get; }

        public ReadOnlyReactiveProperty<Trait> NextTrait { get; }

        public ReadOnlyReactiveProperty<Trait> AfterNextTrait { get; }

        public ReactiveCommand<JoystickUiState> Execute => _execute;

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
