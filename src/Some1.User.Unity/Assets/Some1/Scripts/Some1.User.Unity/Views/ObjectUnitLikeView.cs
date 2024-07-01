using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitLikeView : MonoBehaviour
    {
        private ObjectUnitLikeViewModel _viewModel;
        private Func<UnitElementLike> _getElement;

        public void Setup(ObjectUnitLikeViewModel viewModel, Func<UnitElementLike?> getElement)
        {
            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyValue();
            ApplyCycles();
        }

        private void Start()
        {
            _viewModel.Value.Subscribe(_ => ApplyValue()).AddTo(this);
            _viewModel.Cycles.Subscribe(_ => ApplyCycles()).AddTo(this);
        }

        private void ApplyValue()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            element.spriteRenderer.gameObject.SetActive(_viewModel.Value.CurrentValue);
        }

        private float _lastCycles = 1;
        private void ApplyCycles()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            float cycles = _viewModel.Cycles.CurrentValue;

            TweenHelper.TweenA(element.spriteRenderer, cycles, new(0.3f, 0.5f), 0.2f, 0.3f);

            if (_viewModel.Value.CurrentValue)
            {
                if (cycles <= _lastCycles)
                {
                    element.transform.SetAsLastSibling();
                }
                _lastCycles = cycles;
            }
        }
    }
}
