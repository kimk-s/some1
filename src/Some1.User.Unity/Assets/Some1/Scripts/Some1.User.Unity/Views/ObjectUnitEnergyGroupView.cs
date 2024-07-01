using System;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitEnergyGroupView : MonoBehaviour
    {
        private ObjectUnitEnergyGroupViewModel _viewModel;
        private Func<UnitElementEnergyGroup?> _getElement;

        public void Setup(ObjectUnitEnergyGroupViewModel viewModel, Func<UnitElementEnergyGroup?> getElement)
        {
            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyToScale();
            ApplyHealth();
            ApplyHitsDamageMinimumCycles();
        }

        private void Start()
        {
            _viewModel.CharacterType.CombineLatest(_viewModel.Energies[EnergyId.Health].MaxValue, (x, y) => (x, y))
                .Subscribe(_ => ApplyToScale());

            _viewModel.Energies[EnergyId.Health].NormalizedValue.Subscribe(_ => ApplyHealth()).AddTo(this);

            _viewModel.HitsDamageMinimumCycles.Subscribe(_ => ApplyHitsDamageMinimumCycles()).AddTo(this);
        }

        private void ApplyToScale()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var characterType = _viewModel.CharacterType.CurrentValue;
            var healthMaxValue = _viewModel.Energies[EnergyId.Health].MaxValue.CurrentValue;
            bool viewModel = characterType == CharacterType.NonPlayer && healthMaxValue > 3_000;

            float scale = viewModel ? 1.5f : 1;

            element.transform.localScale = new(scale, scale, 1);
        }

        private void ApplyHealth()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            ApplyEnergy(element.healthBar, EnergyId.Health);

            element.healthText.text = _viewModel.Energies[EnergyId.Health].Value.CurrentValue.ToString();
        }

        private void ApplyEnergy(Transform? bar, EnergyId id)
        {
            if (bar == null)
            {
                return;
            }

            bar.localScale = new(_viewModel.Energies[id].NormalizedValue.CurrentValue, 1, 1);
        }

        private void ApplyHitsDamageMinimumCycles()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            TweenHelper.TweenB(
                element.health,
                _viewModel.HitsDamageMinimumCycles.CurrentValue ?? 0,
                new(1.3f, 1.3f),
                0.1f,
                0.2f);
        }
    }
}
