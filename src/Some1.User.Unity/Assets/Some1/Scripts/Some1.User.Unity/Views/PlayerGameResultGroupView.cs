using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ObservableCollections;
using R3;
using Some1.User.Unity.Components;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerGameResultGroupView : MonoBehaviour
    {
        public GameObject itemViewsContent;
        public PlayerGameResultView itemViewPrefab;
        public TalkLinkButton talkLinkButton;
        public GameObject empty;

        private PlayerGameResultGroupViewModel _viewModel;
        private AsyncLazy<PlayerGameResultDetailView> _detailViewPrefab;
        private ReactiveCommand<int> _openDetailView;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.GameMenu.Results;
            _detailViewPrefab = new(() => ResourcesUtility.LoadViewAsync<PlayerGameResultDetailView>(destroyCancellationToken));
            talkLinkButton.Setup(serviceProvider, popups);
            _openDetailView = new ReactiveCommand<int>().AddTo(this);
            _openDetailView.SubscribeAwait(
                async (x, ct) => popups.AddSingle(await _detailViewPrefab, mayExists: true).Setup(_viewModel.CreateDetail(x)),
                AwaitOperation.Drop);
        }

        public void OpenLastDetail()
        {
            _openDetailView.Execute(_viewModel.GetIdOfLastEndTime());
        }

        private void Start()
        {
            foreach (var item in _viewModel.OrderByDescendingEndTime)
            {
                itemViewsContent.Add(itemViewPrefab).Setup(item, _openDetailView);
            }
            _viewModel.OrderByDescendingEndTime.ObserveMove()
                .Subscribe(x =>
                {
                    itemViewsContent.transform.GetChild(x.OldIndex).SetSiblingIndex(x.NewIndex);
                })
                .AddTo(this);

            _viewModel.OrderByDescendingEndTime.Select(x => x.Active)
                .CombineLatestValuesAreAllFalse()
                .SubscribeToActive(empty)
                .AddTo(this);
        }
    }
}
