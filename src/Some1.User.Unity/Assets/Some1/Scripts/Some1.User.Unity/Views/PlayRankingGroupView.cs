using System;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Resources;
using Some1.User.Unity.Components;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayRankingGroupView : MonoBehaviour, IBackable
    {
        public GameObject itemViewsContent;
        public PlayRankingView itemViewPrefab;
        public TMP_Text timeLeftUntilUpdateText;
        public GameObject empty;
        public Button upButton;
        public TalkLinkButton talkLinkButton;

        private PlayRankingGroupViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.GameMenu.Rankings;
            talkLinkButton.Setup(serviceProvider, popups);
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        private void Start()
        {
            foreach (var item in _viewModel.All)
            {
                itemViewsContent.Add(itemViewPrefab).Setup(item);
            }

            R.Culture
                .CombineLatest(
                    _viewModel.TimeLeftUntilUpdate,
                    (_, x) => StringFormatter.FormatRankingTimeLeftUntilUpdate(x))
                .SubscribeToText(timeLeftUntilUpdateText)
                .AddTo(this);

            _viewModel.Empty.SubscribeToActive(empty).AddTo(this);

            upButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }
    }
}
