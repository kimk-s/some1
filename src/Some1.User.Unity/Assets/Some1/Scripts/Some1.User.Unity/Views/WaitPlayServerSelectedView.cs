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
    public class WaitPlayServerSelectedView : MonoBehaviour
    {
        public Image countryFlagImage;
        public TMP_Text primaryText;
        public TMP_Text secondaryText;
        public Button openPlayServerGroupButton;

        private ReadOnlyReactiveProperty<WaitPlayServerSelectedViewModel?> _viewModel;
        private ReactiveCommand<Unit> _openPlayServerGroup;

        public void Setup(ReadOnlyReactiveProperty<WaitPlayServerSelectedViewModel?> viewModel, ReactiveCommand<Unit> openPlayServerGroup)
        {
            _viewModel = viewModel;
            _openPlayServerGroup = openPlayServerGroup;
        }

        private void Start()
        {
            _viewModel
                .Subscribe(_ => LoadCountryFlagImageAsync().Forget())
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Select(x => x?.Id),
                    (_, id) => id is null ? "" : StringFormatter.FormatServerName(id.Value))
                .SubscribeToText(primaryText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel,
                    (_, x) => x is null ? "" : StringFormatter.FormatServerStatus(x.OpeningSoon, x.Maintenance, x.IsFull))
                .SubscribeToText(secondaryText)
                .AddTo(this);

            _openPlayServerGroup.BindTo(openPlayServerGroupButton).AddTo(this);
        }

        private async UniTaskVoid LoadCountryFlagImageAsync()
        {
            countryFlagImage.sprite = null;

            string? city = _viewModel.CurrentValue?.Id.City;

            if (city is null)
            {
                return;
            }

            var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(
                CountryConverter.GetFlagPath(ServerConverter.GetCountryCode(city)),
                destroyCancellationToken);

            if (city != _viewModel.CurrentValue.Id.City)
            {
                return;
            }

            countryFlagImage.sprite = sprite;
        }
    }
}
