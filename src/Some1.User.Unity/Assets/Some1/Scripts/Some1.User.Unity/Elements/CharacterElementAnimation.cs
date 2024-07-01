using System;
using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class CharacterElementAnimation : MonoBehaviour
    {
        private const float TransitionLength = 0.2f;
        private const float ToleranceLength = 0.05f;

        [SerializeField] private GameObject _modelContainer;
        [SerializeField] private CharacterElementEffectGroup? _effects;
        private Animator _animator;
        private CharacterElementAnimationState? _state;
        private CharacterElementAnimationState? _effectState;
        private float _checkExepectedNormalizedTime;

        public CharacterElementAnimationState? State
        {
            get => _state;

            set
            {
                if (_state == value)
                {
                    return;
                }

                if (_state?.HashName != value?.HashName
                    || _state?.Length != value?.Length
                    || !IsExpectedNormalizedTime())
                {
                    SetAnimator(value);
                }

                _state = value;
            }
        }

        private void Awake()
        {
            _animator = _modelContainer.GetComponentInChildren<Animator>() ?? throw new InvalidOperationException($"Failed to get Animator compoenent in children on {transform.root.name}.");
        }

        private void OnDisable()
        {
            State = null;
            SetEffectState(null);
        }

        private void LateUpdate()
        {
            SetEffectState(State);
        }

        private void SetEffectState(CharacterElementAnimationState? value)
        {
            if (_effectState == value)
            {
                return;
            }

            if (_effects != null && value is not null)
            {
                if (_effectState is null
                    || _effectState.Value.Status != value.Value.Status
                    || _effectState.Value.NormalizedTime > value.Value.NormalizedTime
                    || MathF.Truncate(_effectState.Value.NormalizedTime) != MathF.Truncate(value.Value.NormalizedTime))
                {
                    _effects.Play(value.Value.Status.ToEffectId());
                }
            }

            _effectState = value;
        }

        private bool IsExpectedNormalizedTime()
        {
            if (State == null || Time.time <= _checkExepectedNormalizedTime)
            {
                return true;
            }

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
#if UNITY_EDITOR
            if (MathF.Abs(stateInfo.length - State.Value.Length) > ToleranceLength)
            {
                Debug.LogWarning($"Character Animator '{State.Value.Name}({stateInfo.IsName(State.Value.Name)})' state length is not equal ({stateInfo.length}, {State.Value.Length}) in '{name}'.");
            }
#endif
            _checkExepectedNormalizedTime = Time.time + TransitionLength * 1.5f;

            float expected = stateInfo.normalizedTime * stateInfo.length + Time.deltaTime;
            float actual = State.Value.NormalizedTime * stateInfo.length;
            return MathF.Abs(expected - actual) <= ToleranceLength;
        }

        private void SetAnimator(CharacterElementAnimationState? state)
        {
            if (state == null)
            {
                _animator.StopPlayback();
            }
            else
            {
#if UNITY_EDITOR
                if (!_animator.HasState(0, state.Value.HashName))
                {
                    Debug.LogWarning($"Character Animator '{state.Value.Name}' state is not in '{name}'.");
                }
#endif
                _checkExepectedNormalizedTime = Time.time + TransitionLength * 1.5f;

                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                _animator.CrossFade(state.Value.HashName, TransitionLength / stateInfo.length, -1, state.Value.NormalizedTime);
            }
        }
    }
}
