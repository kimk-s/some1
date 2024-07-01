using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using R3;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerGameArgsGroupView : MonoBehaviour
    {
        public GameObject itemViewsContent;
        public PlayerGameArgsView itemViewPrefab;

        private IServiceProvider _serviceProvider;
        private PlayerGameArgsGroupViewModel _viewModel;
        private GameObject _popups;
        private AsyncLazy<PlayRankingGroupView> _rankingsViewPrefab;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _serviceProvider = serviceProvider;
            _viewModel = _serviceProvider.GetRequiredService<PlayViewModel>().Popups.GameMenu.Argses;
            _popups = popups;
            _rankingsViewPrefab = new(() => ResourcesUtility.LoadViewAsync<PlayRankingGroupView>(destroyCancellationToken));
        }

        private void Start()
        {
            var openRankingsView = new ReactiveCommand<Unit>().AddTo(this);
            openRankingsView.SubscribeAwait(
                async (_, ct) => _popups.AddSingle(await _rankingsViewPrefab).Setup(_serviceProvider, _popups),
                AwaitOperation.Drop);

            foreach (var item in _viewModel.All.Values)
            {
                itemViewsContent.Add(itemViewPrefab).Setup(item, openRankingsView);
            }
        }
    }
}
