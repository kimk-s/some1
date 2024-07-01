using System;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitCastView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposablesOnViewModelChanged = new();
        private ReadOnlyReactiveProperty<ObjectUnitCastViewModel?> _viewModel;
        private Func<UnitElementCast?> _getElement;

        public void Setup(ReadOnlyReactiveProperty<ObjectUnitCastViewModel?> viewModel, Func<UnitElementCast?> getElement)
        {
            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyAttackNormalizedLoadWave();
            ApplyAttackNormalizedLoadCount();
            ApplyAttackMaxLoadCount();

            ApplySuperAnyLoadCount();
            ApplySuperIconText();

            ApplyUltraAnyLoadCount();
            ApplyUltraIconText();
        }

        private void Start()
        {
            _viewModel.Subscribe(x =>
            {
                _disposablesOnViewModelChanged.Clear();

                if (x is null)
                {
                    return;
                }

                {
                    var attack = x.Items[CastId.Attack];
                    attack.NormalizedLoadWave
                        .Subscribe(_ => ApplyAttackNormalizedLoadWave())
                        .AddTo(_disposablesOnViewModelChanged);
                    attack.LoadCount
                        .CombineLatest(attack.MaxLoadCount, (a, b) => (a, b))
                        .Subscribe(_ => ApplyAttackNormalizedLoadCount())
                        .AddTo(_disposablesOnViewModelChanged);
                    attack.MaxLoadCount
                        .Subscribe(_ => ApplyAttackMaxLoadCount())
                        .AddTo(_disposablesOnViewModelChanged);
                }

                {
                    var super = x.Items[CastId.Super];
                    super.AnyLoadCount
                        .Subscribe(_ => ApplySuperAnyLoadCount())
                        .AddTo(_disposablesOnViewModelChanged);
                }

                {
                    var ultra = x.Items[CastId.Ultra];
                    ultra.AnyLoadCount
                        .Subscribe(_ => ApplyUltraAnyLoadCount())
                        .AddTo(_disposablesOnViewModelChanged);
                }

                {
                    x.NextTrait
                        .Subscribe(_ => ApplyUltraIconText())
                        .AddTo(_disposablesOnViewModelChanged);
                }
            }).AddTo(this);

            _disposablesOnViewModelChanged.AddTo(this);
        }

        private void ApplyAttackNormalizedLoadWave()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var viewModel = _viewModel.CurrentValue;
            float value = viewModel == null ? 0 : viewModel.Items[CastId.Attack].NormalizedLoadWave.CurrentValue;

            element.attackNormalizedLoadWaveBar.localScale = new(value, 1, 1);
        }

        private void ApplyAttackNormalizedLoadCount()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var viewModel = _viewModel.CurrentValue?.Items[CastId.Attack];
            int loadCount = viewModel == null ? 0 : viewModel.LoadCount.CurrentValue;
            int maxLoadCount = viewModel == null ? 0 : viewModel.MaxLoadCount.CurrentValue;
            float value = maxLoadCount == 0 ? 0 : (float)loadCount / maxLoadCount;

            element.attackNormalizedLoadCountBar.localScale = new(value, 1, 1);
        }

        private void ApplyAttackMaxLoadCount()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var viewModel = _viewModel.CurrentValue?.Items[CastId.Attack];
            int value = viewModel == null ? 0 : viewModel.MaxLoadCount.CurrentValue;

            element.attackTwoMaxLoadCount.SetActive(value == 2);
            element.attackThreeMaxLoadCount.SetActive(value == 3);
            element.attackFourMaxLoadCount.SetActive(value == 4);
            element.attackFiveMaxLoadCount.SetActive(value == 5);
        }

        private void ApplySuperAnyLoadCount()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var viewModel = _viewModel.CurrentValue?.Items[CastId.Super];
            bool value = viewModel != null && viewModel.AnyLoadCount.CurrentValue;

            element.superAnyLoadCount.SetActive(value);
        }

        private void ApplySuperIconText()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            element.superIconText.text = SkillId.Skill2.GetIconString();
        }

        private void ApplyUltraAnyLoadCount()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var viewModel = _viewModel.CurrentValue;
            bool value = viewModel != null && viewModel.Items[CastId.Ultra].AnyLoadCount.CurrentValue;

            element.ultraAnyLoadCount.SetActive(value);
        }

        private void ApplyUltraIconText()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var value = _viewModel.CurrentValue?.NextTrait.CurrentValue ?? Trait.None;

            element.ultraIconText.text =  value.GetIconString();
        }
    }
}
