using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Elements;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitTakeStuffView : MonoBehaviour
    {
        private ObjectUnitTakeStuffViewModel _viewModel;
        private Func<UnitElementTakeStuff> _getElement;

        public void Setup(ObjectUnitTakeStuffViewModel viewModel, Func<UnitElementTakeStuff> getElement)
        {
            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyStateAsync().Forget();
            ApplyCycles();
        }

        private void Start()
        {
            _viewModel.Stuff.Subscribe(_ => ApplyStateAsync().Forget()).AddTo(this);
            _viewModel.Cycles.Subscribe(_ => ApplyCycles()).AddTo(this);
        }

        private async UniTaskVoid ApplyStateAsync()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            element.spriteRenderer.gameObject.SetActive(false);
            element.spriteRenderer.sprite = null;

            var stuff = _viewModel.Stuff.CurrentValue;
            if (stuff is null)
            {
                return;
            }

            var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(stuff.Value.GetIconPath(), destroyCancellationToken);

            var element2 = _getElement();
            if (element2 != element || stuff != _viewModel.Stuff.CurrentValue)
            {
                return;
            }

            element2.spriteRenderer.gameObject.SetActive(true);
            element2.spriteRenderer.sprite = sprite;
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

            TweenHelper.TweenA(element.spriteRenderer, cycles, new(-0.3f, 0.5f), 0.2f, 0.3f);

            if (_viewModel.Stuff.CurrentValue != null)
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
