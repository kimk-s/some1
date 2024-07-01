using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectCharacterView : MonoBehaviour
    {
        public ElementManager elementManager;

        private ReactiveProperty<Vector2> _size;
        private ObjectCharacterViewModel _viewModel;
        private ReadOnlyReactiveProperty<CharacterElementAnimationState> _animationState;

        public ReadOnlyReactiveProperty<Vector2> Size => _size;

        public void Setup(ObjectCharacterViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Awake()
        {
            _size = new ReactiveProperty<Vector2>().AddTo(this);
        }

        private void Start()
        {
            _viewModel.Transform.Position.Subscribe(_ => ApplyTransformPosition()).AddTo(this);

            float? damageMinimumCycles = null;
            _viewModel.Hits.DamageMinimumCycles
                .Subscribe(x =>
                {
                    var element = GetElement();
                    if (element == null || element.effects == null)
                    {
                        return;
                    }

                    if (x is not null)
                    {
                        if (damageMinimumCycles is null || x.Value < damageMinimumCycles.Value)
                        {
                            element.effects.Play(CharacterElementEffectId.Damage);
                        }
                    }

                    damageMinimumCycles = x;
                })
                .AddTo(this);

            _viewModel.Shift.Height
                .CombineLatest(_viewModel.Hits.DamagePush, (a, b) => (a, b))
                .Subscribe(_ => ApplyToBasePosition())
                .AddTo(this);

            _viewModel.Transform.Rotation.Subscribe(_ => ApplyToBaseRotation()).AddTo(this);

            _animationState = _viewModel.Alive.Alive.CombineLatest(_viewModel.Alive.Cycles, _viewModel.Alive.Cycle, (value, cycles, cycle) => (value, cycles, cycle))
                .CombineLatest(
                    _viewModel.Idle.Idle.CombineLatest(_viewModel.Idle.Cycles, _viewModel.Idle.Cycle, (value, cycles, cycle) => (value, cycles, cycle)),
                    _viewModel.Shift.Shift.CombineLatest(_viewModel.Shift.SecondaryCycles, _viewModel.Shift.Cycle, (value, secondaryCycles, cycle) => (value, secondaryCycles, cycle)),
                    _viewModel.Cast.Cast.CombineLatest(_viewModel.Cast.Cycles, _viewModel.Cast.Cycle, (value, cycles, cycle) => (value, cycles, cycle)),
                    _viewModel.Walk.Walk.CombineLatest(_viewModel.Walk.Cycles, _viewModel.Walk.Cycle, (value, cycles, cycle) => (value, cycles, cycle)),
                    (alive, idle, shift, cast, walk) =>
                    {
                        if (!alive.value)
                        {
                            return new CharacterElementAnimationState(
                                CharacterElementAnimationStatus.Dead,
                                alive.cycle,
                                alive.cycles);
                        }
                        else if (shift.value is not null)
                        {
                            return new CharacterElementAnimationState(
                                shift.value.Value.Id switch
                                {
                                    ShiftId.Jump => CharacterElementAnimationStatus.Jump,
                                    ShiftId.Knock => CharacterElementAnimationStatus.Knock,
                                    ShiftId.Dash => CharacterElementAnimationStatus.Dash,
                                    ShiftId.Grab => CharacterElementAnimationStatus.Grab,
                                    _ => throw new InvalidOperationException()
                                },
                                shift.cycle,
                                shift.secondaryCycles);
                        }
                        else if (cast.value is not null)
                        {
                            return new CharacterElementAnimationState(
                                cast.value.Value.Id switch
                                {
                                    CastId.Attack => CharacterElementAnimationStatus.Attack,
                                    CastId.Super => CharacterElementAnimationStatus.Super,
                                    CastId.Ultra => CharacterElementAnimationStatus.Ultra,
                                    _ => throw new InvalidOperationException()
                                },
                                cast.cycle,
                                cast.cycles);
                        }
                        else if (walk.value is not null)
                        {
                            return new CharacterElementAnimationState(
                                CharacterElementAnimationStatus.Walk,
                                walk.cycle,
                                walk.cycles);
                        }
                        else if (alive.cycles <= 1)
                        {
                            return new CharacterElementAnimationState(
                                CharacterElementAnimationStatus.Alive,
                                alive.cycle,
                                alive.cycles);
                        }
                        else
                        {
                            return new CharacterElementAnimationState(
                                CharacterElementAnimationStatus.Idle,
                                idle.cycle,
                                idle.cycles);
                        }
                    })
                .ToReadOnlyReactiveProperty()
                .AddTo(this);

            _animationState.Subscribe(_ => ApplyAnimationState());

            _viewModel.GiveStuff.IsTaken.Subscribe(_ => ApplyGiveStuffIsTaken()).AddTo(this);

            _viewModel.Alive.Cycles.Subscribe(_ => ApplyGiveStuffAliveTime()).AddTo(this);

            elementManager.Register(() =>
            {
                ApplySize();
                ApplyTransformPosition();
                ApplyToBasePosition();
                ApplyToBaseRotation();
                ApplyAnimationState();
                ApplyGiveStuffIsTaken();
                ApplyGiveStuffAliveTime();
            });

            _viewModel.Id.Subscribe(x => elementManager.Path = x?.GetElementPath()).AddTo(this);
        }

        private CharacterElement? GetElement() => (CharacterElement?)elementManager.Element;

        private void ApplySize()
        {
            var element = GetElement();
            if (element == null)
            {
                _size.Value = Vector2.zero;
                return;
            }

            _size.Value = element.size;
        }

        private void ApplyTransformPosition()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.transform.localPosition = _viewModel.Transform.Position.CurrentValue.ToUnityVector3();
        }

        private void ApplyToBasePosition()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.@base.transform.localPosition = _viewModel.Hits.DamagePush.CurrentValue.ToUnityVector3(_viewModel.Shift.Height.CurrentValue);
        }

        private void ApplyToBaseRotation()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.@base.transform.localRotation = _viewModel.Transform.Rotation.CurrentValue.ToUnityQuaternion();
        }

        private void ApplyAnimationState()
        {
            var element = GetElement();
            if (element == null || element.animation == null)
            {
                return;
            }

            element.animation.State = _animationState.CurrentValue;
        }

        private void ApplyGiveStuffIsTaken()
        {
            var element = GetElement();
            if (element == null || element.stuff == null)
            {
                return;
            }

            element.stuff.IsTaken = _viewModel.GiveStuff.IsTaken.CurrentValue;
        }

        private void ApplyGiveStuffAliveTime()
        {
            var element = GetElement();
            if (element == null || element.stuff == null)
            {
                return;
            }

            element.stuff.AliveCycles = _viewModel.Alive.Cycles.CurrentValue;
        }
    }
}
