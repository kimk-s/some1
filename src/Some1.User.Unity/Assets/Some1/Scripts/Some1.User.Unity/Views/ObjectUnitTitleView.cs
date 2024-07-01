using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitTitleView : MonoBehaviour
    {
        private ObjectUnitTitleViewModel _viewModel;
        private Func<UnitElementTitle?> _getElement;

        public void Setup(ObjectUnitTitleViewModel viewModel, Func<UnitElementTitle?> getElement)
        {
            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyStarGrade();
            ApplyPlayerId();
        }

        private void Start()
        {
            _viewModel.StarGrade.Subscribe(_ => ApplyStarGrade()).AddTo(this);
            _viewModel.PlayerId.Subscribe(_ => ApplyPlayerId()).AddTo(this);
        }

        private void ApplyStarGrade()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var value = _viewModel.StarGrade.CurrentValue;

            element.starText.text = value?.Point.ToString();
            element.starMax.SetActive(value?.IsMaxLevel == true);
        }

        private void ApplyPlayerId()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            var value = _viewModel.PlayerId.CurrentValue;

            element.playerIdText.text = value?.GetMark();
            element.playerIdSpriteRenderer.color = value?.GetColor() ?? Color.gray;
        }
    }
}
