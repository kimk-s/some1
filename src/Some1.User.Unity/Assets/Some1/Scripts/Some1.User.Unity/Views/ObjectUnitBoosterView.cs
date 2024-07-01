using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitBoosterView : MonoBehaviour
    {
        private ObjectUnitBoosterViewModel _viewModel;
        private Func<UnitElementBooster?> _getElement;

        public void Setup(ObjectUnitBoosterViewModel viewModel, Func<UnitElementBooster?> getElement)
        {
            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyNumber();
            ApplyNormalizedConsumingDelay();
        }

        private void Start()
        {
            _viewModel.Number.Subscribe(_ => ApplyNumber()).AddTo(this);
            _viewModel.NormalizedConsumingDelay.Subscribe(_ => ApplyNormalizedConsumingDelay()).AddTo(this);
            _viewModel.Time.Subscribe(_ => ApplyTime()).AddTo(this);
        }

        private void ApplyNumber()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            element.text.text = StringFormatter.FormatBoosterNumber(_viewModel.Number.CurrentValue);
        }

        private void ApplyNormalizedConsumingDelay()
        {
            var element = _getElement();
            if (element == null || element.bar == null)
            {
                return;
            }

            element.bar.localScale = new(1, _viewModel.NormalizedConsumingDelay.CurrentValue, 1);
        }

        private void ApplyTime()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            TweenHelper.TweenB(
                element.transform,
                _viewModel.Time.CurrentValue,
                new(1.2f, 1.2f));
        }
    }
}
