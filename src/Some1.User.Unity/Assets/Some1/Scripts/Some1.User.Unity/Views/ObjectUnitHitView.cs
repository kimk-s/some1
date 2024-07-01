using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitHitView : MonoBehaviour
    {
        private ObjectUnitHitViewModel _viewModel;
        private Func<(UnitElementHit hit, ColorId damageColor, ColorId recoveryColor)?> _getElement;
        private ReadOnlyReactiveProperty<bool> _active;

        public void Setup(ObjectUnitHitViewModel viewModel, Func<(UnitElementHit hit, ColorId damageColor, ColorId recoveryColor)?> getElement)
        {
            _active = viewModel.Hit
                .CombineLatest(
                    viewModel.ToMe,
                    viewModel.FromMe,
                    (hit, toMe, fromMe) => hit is not null && (toMe || fromMe))
                .ToReadOnlyReactiveProperty()
                .AddTo(this);

            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyActive();
            ApplyHit();
            ApplyCycles();
        }

        private void Start()
        {
            _active.Subscribe(_ => ApplyActive()).AddTo(this);
            _viewModel.Hit.Subscribe(_ => ApplyHit()).AddTo(this);
            _viewModel.Cycles.Subscribe(_ => ApplyCycles()).AddTo(this);
        }

        private void ApplyActive()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            element.Value.hit.text.gameObject.SetActive(_active.CurrentValue);
        }

        private void ApplyHit()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            element.Value.hit.text.text = _viewModel.Hit.CurrentValue?.Value.ToString();
            element.Value.hit.text.color = R.GetColor(_viewModel.Hit.CurrentValue?.Id.IsDamage() == true
                ? element.Value.damageColor
                : element.Value.recoveryColor
            ).ToUnityColor(element.Value.hit.text.color.a);
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

            TweenHelper.TweenA(element.Value.hit.text, cycles, new(0, 0.5f));

            if (_viewModel.Hit != null)
            {
                if (cycles <= _lastCycles)
                {
                    element.Value.hit.transform.SetAsLastSibling();
                }
                _lastCycles = cycles;
            }
        }
    }
}
