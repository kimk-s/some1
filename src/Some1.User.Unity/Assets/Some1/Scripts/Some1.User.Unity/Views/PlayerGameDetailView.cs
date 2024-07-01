using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ObservableCollections;
using R3;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Components;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameDetailView : MonoBehaviour, IBackable
    {
        public TMP_Text modeIconText;
        public TMP_Text modeNameText;
        public TMP_Text modeDescriptionText;
        public TMP_Text scoreText;
        public GameObject timeRemainingContainer;
        public TMP_Text timeRemainingText;
        public GameObject elapsedTimeContainer;
        public TMP_Text elapsedTimeText;
        public TMP_Text specialtyCountText;
        public GameObject specialtyViewsContent;
        public PlayerGameSpecialtyView specialtyViewPrefab;
        public GameObject challengeReturnContainer;
        public TMP_Text challengeReturnText;
        public GameObject adventureReturnContainer;
        public Button adventureReturnButton;
        public Button closeButton;
        public Button backButton;
        public GameObject popups;
        public MessageListView messageList;
        public TalkLinkButton talkLinkButton;

        private PlayerGameDetailViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.GameDetail;
            messageList.Setup(_viewModel.MessageList);
            talkLinkButton.Setup(serviceProvider);
        }

        public bool Back()
        {
            if (gameObject.activeSelf)
            {
                if (messageList.Back())
                {
                    return true;
                }

                if (BackUtility.Back(popups))
                {
                    return true;
                }

                Close();
                return true;
            }

            return false;
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _viewModel.Close.Subscribe(_ => Back()).AddTo(this);

            _viewModel.Game
                .Select(x => x is null ? "" : x.Value.Mode.GetIconString())
                .SubscribeToText(modeIconText)
                .AddTo(this);

            _viewModel.Game
                .Select(x => x is null ? "" : x.Value.Mode.GetName())
                .AsRStringObservable()
                .SubscribeToText(modeNameText)
                .AddTo(this);

            _viewModel.Game
                .Select(x => x is null ? "" : x.Value.Mode.GetDescription())
                .AsRStringObservable()
                .SubscribeToText(modeDescriptionText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Game,
                    (_, game) => StringFormatter.FormatScore(game?.Score))
                .SubscribeToText(scoreText)
                .AddTo(this);

            _viewModel.Game.Select(x => x?.Mode == GameMode.Challenge).SubscribeToActive(timeRemainingContainer).AddTo(this);

            var time = _viewModel.Time
                .Select(x => TimeSpan.FromSeconds(Math.Ceiling(x.TotalSeconds)))
                .ToReadOnlyReactiveProperty()
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    time,
                    (_, x) => StringFormatter.FormatTimeSpanShort(x))
                .SubscribeToText(timeRemainingText)
                .AddTo(this);

            _viewModel.Game.Select(x => x?.Mode != GameMode.Challenge).SubscribeToActive(elapsedTimeContainer).AddTo(this);

            R.Culture
                .CombineLatest(
                    time,
                    (_, x) => StringFormatter.FormatTimeSpanShort(x))
                .SubscribeToText(elapsedTimeText)
                .AddTo(this);

            _viewModel.SpecialtyActiveCount
                .Select(x => StringFormatter.FormatGameSpecialtyCount(x))
                .SubscribeToText(specialtyCountText)
                .AddTo(this);

            foreach (var item in _viewModel.SpecialtiesOrdered)
            {
                specialtyViewsContent.Add(specialtyViewPrefab).Setup(item);
            }
            _viewModel.SpecialtiesOrdered.ObserveMove()
                .Subscribe(x => specialtyViewsContent.transform.GetChild(x.OldIndex).SetSiblingIndex(x.NewIndex))
                .AddTo(this);

            _viewModel.ManagerStatus
                .Skip(1)
                .Where(x => x.IsRuning())
                .Subscribe(_ => Close())
                .AddTo(this);

            _viewModel.Game.Select(x => x?.Mode == GameMode.Challenge).SubscribeToActive(challengeReturnContainer).AddTo(this);

            R.Culture
                .CombineLatest(
                    time,
                    (_, x) => StringFormatter.FormatReturnAfter(x))
                .SubscribeToText(challengeReturnText)
                .AddTo(this);

            _viewModel.Game.Select(x => x?.Mode != GameMode.Challenge).SubscribeToActive(adventureReturnContainer).AddTo(this);

            _viewModel.Return.BindTo(adventureReturnButton).AddTo(this);

            _viewModel.ReturnSuccess.Subscribe(_ => Close()).AddTo(this);

            closeButton.OnClickAsObservable().Subscribe(_ => Close()).AddTo(this);

            backButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }
    }
}
