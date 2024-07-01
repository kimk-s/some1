using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Elements;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitMedalView : MonoBehaviour
    {
        private ObjectUnitMedalViewModel _viewModel;
        private Func<UnitElementMedal?> _getElement;

        public void Setup(ObjectUnitMedalViewModel viewModel, Func<UnitElementMedal?> getElement)
        {
            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyValueAsync().Forget();
        }

        private void Start()
        {
            _viewModel.Value.Subscribe(_ => Apply()).AddTo(this);
        }

        private async UniTaskVoid ApplyValueAsync()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            element.gameObject.SetActive(false);
            element.particleSystemRenderer.material = null;

            var value = _viewModel.Value.CurrentValue;

            if (value is null || value == Medal.None)
            {
                return;
            }

            var material = await ResourcesUtility.LoadSafeAsync<Material>(value.Value.GetFXMaterialPath(), destroyCancellationToken);

            var element2 = _getElement();
            if (element2 != element || value != _viewModel.Value.CurrentValue)
            {
                return;
            }

            element2.gameObject.SetActive(true);
            element2.particleSystemRenderer.material = material;
        }
    }
}
