using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class TalkGroupView : MonoBehaviour, IBackable
    {
        public GameObject itemViewsContainer;
        public TalkView itemViewPrefab;
        public Button upButton;

        private TalkGroupViewModel _viewModel;
        private GameObject _popups;

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _viewModel = serviceProvider.GetService<TalkGroupViewModel>();
            _popups = popups;
        }

        private void Start()
        {
            var detailViewPrefab = new AsyncLazy<TalkDetailView>(
                () => ResourcesUtility.LoadViewAsync<TalkDetailView>(destroyCancellationToken));

            var openDetail = new ReactiveCommand<TalkId>().AddTo(this);
            openDetail
                .ThrottleFirst(TimeSpan.FromSeconds(0.1f))
                .SubscribeAwait(
                    async (x, ct) => _popups.AddSingle(await detailViewPrefab).Setup(_viewModel.CreateDetail(x)),
                    AwaitOperation.Drop);

            foreach (var item in _viewModel.Items.Values)
            {
                itemViewsContainer.Add(itemViewPrefab).Setup(item, openDetail);
            }

            upButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }
    }
}
