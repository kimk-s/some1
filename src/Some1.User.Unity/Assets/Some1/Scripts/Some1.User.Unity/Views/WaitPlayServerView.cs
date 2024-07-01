using Cysharp.Threading.Tasks;
using R3;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class WaitPlayServerView : MonoBehaviour
    {
        public Image countryFlagImage;
        public TMP_Text primaryText;
        public TMP_Text secondaryText;
        public GameObject isSelected;
        public GameObject disabled;
        public Button selectButton;

        private WaitPlayServerViewModel _viewModel;

        public void Setup(WaitPlayServerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            LoadCountryFlagImageAsync().Forget();

            R.Culture
                .Select(_ => StringFormatter.FormatServerName(_viewModel.Id))
                .SubscribeToText(primaryText)
                .AddTo(this);

            R.Culture
                .Select(_ => StringFormatter.FormatServerStatus(_viewModel.OpeningSoon, _viewModel.Maintenance, _viewModel.IsFull))
                .SubscribeToText(secondaryText)
                .AddTo(this);

            isSelected.SetActive(_viewModel.IsSelected);

            disabled.SetActive(_viewModel.OpeningSoon | _viewModel.Maintenance | _viewModel.IsFull);

            _viewModel.Select.BindTo(selectButton).AddTo(this);
        }

        private async UniTaskVoid LoadCountryFlagImageAsync()
        {
            countryFlagImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(
                CountryConverter.GetFlagPath(ServerConverter.GetCountryCode(_viewModel.Id.City)),
                destroyCancellationToken);
        }
    }
}
