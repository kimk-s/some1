using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Components;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerCharacterGroupView : MonoBehaviour
    {
        public TMP_Text pickTimeRemainedText;
        public TMP_Text starText;
        public PlayerCharacterView itemPrefab;
        public GameObject itemViewsContainer;
        public TalkLinkButton talkLinkButton;

        private PlayerCharacterGroupViewModel _viewModel;
        private GameObject _popups;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.Menu.Characters;
            _popups = popups;
            talkLinkButton.Setup(serviceProvider, popups);
        }

        private void Start()
        {
            var detailViewPrefab = new AsyncLazy<PlayerCharacterDetailView>(
                () => ResourcesUtility.LoadViewAsync<PlayerCharacterDetailView>(destroyCancellationToken));

            var openDetail = new ReactiveCommand<CharacterId>().AddTo(this);
            openDetail.SubscribeAwait(
                async (x, ct) => _popups.AddSingle(await detailViewPrefab).Setup(_viewModel.CreateDetail(x)),
                AwaitOperation.Drop);

            R.Culture
                .CombineLatest(
                    _viewModel.PickTimeRemained,
                    (_, pickedTimeRemained) => StringFormatter.FormatPickTimeRemained(pickedTimeRemained))
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .SubscribeToText(pickTimeRemainedText);

            _viewModel.Star.Select(x => $"{StringFormatter.FormatDetailPointWithStar(x)}")
                .SubscribeToText(starText)
                .AddTo(this);

            foreach (var item in _viewModel.Items.Values)
            {
                itemViewsContainer.Add(itemPrefab).Setup(item, openDetail);
            }
        }
    }
}
