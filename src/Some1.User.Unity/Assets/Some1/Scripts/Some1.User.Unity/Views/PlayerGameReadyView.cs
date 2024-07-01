using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameReadyView : MonoBehaviour
    {
        public TMP_Text modeIconText;
        public TMP_Text modeNameText;
        public GameObject readyBlocker;
        public TMP_Text expText;
        public RectTransform expChargeTimeRemainedBar;
        public Button readyButton;
        public Button cancelButton;

        private PlayerGameReadyViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page.GameReady;
            _viewModel.Game.Select(x => x is null).SubscribeToActive(gameObject).AddTo(this);
        }

        private void Start()
        {
            _viewModel.SelectedArgs
                .Select(x => x?.Id.GetIconString() ?? string.Empty)
                .SubscribeToText(modeIconText)
                .AddTo(this);

            _viewModel.SelectedArgs
                .Select(x => x?.Id.GetName() ?? string.Empty)
                .AsRStringObservable()
                .SubscribeToText(modeNameText)
                .AddTo(this);

            _viewModel.ManagerStatus
                .Select(x => x.IsRuning())
                .SubscribeToActive(readyBlocker)
                .AddTo(this);

            _viewModel.ManagerStatus
                .Select(x => !x.IsRuning())
                .SubscribeToActive(readyButton.gameObject)
                .AddTo(this);

            _viewModel.ManagerStatus
                .Select(x => x.IsRuning())
                .SubscribeToActive(cancelButton.gameObject)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.ExpValue,
                    _viewModel.ExpChargeTimeRemained,
                    (_, value, chargeTimeRemained) => StringFormatter.FormatPlayerExp(value, chargeTimeRemained))
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .SubscribeToText(expText);

            _viewModel.ExpValue
                .Select(x => Math.Clamp((float)x / PlayConst.MaxPlayerExp, 0, 1))
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(x => expChargeTimeRemainedBar.localScale = new(x, 1, 1));

            _viewModel.Ready.BindTo(readyButton).AddTo(this);

            _viewModel.Cancel.BindTo(cancelButton).AddTo(this);
        }
    }
}
