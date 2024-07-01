using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitTakeStuffGroupView : MonoBehaviour
    {
        public ObjectUnitTakeStuffView[] items;

        private ObjectUnitTakeStuffGroupViewModel _viewModel;
        private Func<UnitElementTakeStuffGroup?> _getElement;

        public void Setup(ObjectUnitTakeStuffGroupViewModel viewModel, Func<UnitElementTakeStuffGroup?> getElement)
        {
            Debug.Assert(items.Length == viewModel.All.Count);
            for (int i = 0; i < items.Length; i++)
            {
                int index = i;
                items[i].Setup(
                    viewModel.All[i],
                    () =>
                    {
                        var element = getElement();
                        return element == null ? null : element.items[index];
                    });
            }

            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            foreach (var item in items)
            {
                item.Apply();
            }
            ApplyComboScore();
            ApplyComboCycles();
        }

        private void Start()
        {
            _viewModel.ComboScore.Subscribe(_ => ApplyComboScore()).AddTo(this);
            _viewModel.ComboCycles.Subscribe(_ => ApplyComboCycles()).AddTo(this);
        }

        private void ApplyComboScore()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            int value = _viewModel.ComboScore.CurrentValue;
            if (value == 0)
            {
                element.comboScoreText.gameObject.SetActive(false);
            }
            else
            {
                element.comboScoreText.text = _viewModel.ComboScore.CurrentValue.ToString();
                element.comboScoreText.gameObject.SetActive(true);
            }
        }

        private void ApplyComboCycles()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            TweenHelper.TweenB(
                element.comboScoreText,
                _viewModel.ComboCycles.CurrentValue,
                new(1.3f, 1.3f),
                0.075f,
                0.15f);
        }
    }
}
