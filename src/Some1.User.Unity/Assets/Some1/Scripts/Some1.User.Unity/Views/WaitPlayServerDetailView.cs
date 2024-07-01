using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class WaitPlayServerDetailView : MonoBehaviour, IBackable
    {
        public PrefsBottomView prefsBottomView;
        public Image countryFlagImage;
        public TMP_Text primaryText;
        public TMP_Text secondaryText;
        public GameObject channelViewsContainer;
        public WaitPlayChannelView channelViewPrefab;
        public Button upButton;

        private WaitPlayServerDetailViewModel _viewModel;

        public void Setup(IServiceProvider services)
        {
            _viewModel = services.GetRequiredService<WaitViewModel>().CreateWaitPlayServerDetailViewModel().AddTo(this);
            prefsBottomView.Setup(services);
        }

        public bool Back()
        {
            _viewModel.Back.Execute(Unit.Default);
            return true;
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

            foreach (var item in _viewModel.Channels)
            {
                channelViewsContainer.Add(channelViewPrefab).Setup(item);
            }

            _viewModel.Back.BindTo(upButton).AddTo(this);
            _viewModel.Back.Subscribe(_ => Destroy(gameObject)).AddTo(this);
        }

        private async UniTaskVoid LoadCountryFlagImageAsync()
        {
            countryFlagImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(
                CountryConverter.GetFlagPath(ServerConverter.GetCountryCode(_viewModel.Id.City)),
                destroyCancellationToken);
        }
    }
}
