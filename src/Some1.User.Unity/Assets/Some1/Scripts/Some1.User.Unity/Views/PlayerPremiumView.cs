using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerPremiumView : MonoBehaviour
    {
        public TMP_Text isPremiumText;
        public GameObject timeLeftContainer;
        public TMP_Text timeLeftText;
        public GameObject productsContent;
        public PlayerProductView productViewPrefab;
        public Button openLogGroupView;

        private PlayerPremiumViewModel _viewModel;
        private AsyncLazy<PlayPremiumLogGroupView> _logGroupViewPrefab;
        private AsyncLazy<PlayerPremiumErrorView> _errorViewPrefab;
        private GameObject _popups;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.Menu.Premium;
            _popups = popups;
            _logGroupViewPrefab = new(() => ResourcesUtility.LoadViewAsync<PlayPremiumLogGroupView>(destroyCancellationToken));
            _errorViewPrefab = new(() => ResourcesUtility.LoadViewAsync<PlayerPremiumErrorView>(destroyCancellationToken));
        }

        private void Start()
        {
            var timeLeftSeconds = _viewModel.TimeLeft
                .Select(x => TimeSpan.FromSeconds((int)Math.Ceiling(x.TotalSeconds)))
                .ToReadOnlyReactiveProperty()
                .AddTo(this);

            var isPremium = timeLeftSeconds.Select(x => x.TotalSeconds > 0).ToReadOnlyReactiveProperty().AddTo(this);
            isPremium.Select(x => CommonConverter.GetYesNo(x)).AsRStringObservable().SubscribeToText(isPremiumText).AddTo(this);
            isPremium.SubscribeToActive(timeLeftContainer).AddTo(this);
            isPremium.Subscribe(_ => LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(timeLeftContainer)).AddTo(this);

            R.Culture
                .CombineLatest(
                    timeLeftSeconds,
                    (_, x) => StringFormatter.FormatTimeSpanShort(x))
                .SubscribeToText(timeLeftText)
                .AddTo(this);

            foreach (var item in _viewModel.Products.Values)
            {
                productsContent.Add(productViewPrefab).Setup(item);
            }
            LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(productsContent);

            var openCommand = new ReactiveCommand<Unit>().AddTo(this);
            openCommand.SubscribeAwait(
                async (_, ct) => _popups.AddSingle(await _logGroupViewPrefab).Setup(_viewModel.CreatePremiumLogGroup(), _popups),
                AwaitOperation.Drop);
            openCommand.BindTo(openLogGroupView);

            _viewModel.BuyState
                .Where(x => x.Exception is IAPSecurityException)
                .SubscribeAwait(
                    async (_, ct) => _popups.AddSingle(await _errorViewPrefab).Setup(_viewModel.BuyState),
                    AwaitOperation.Drop)
                .AddTo(this);
        }
    }
}
