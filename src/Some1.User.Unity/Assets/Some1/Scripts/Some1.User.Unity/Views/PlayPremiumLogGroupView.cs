using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayPremiumLogGroupView : MonoBehaviour, IBackable
    {
        public GameObject itemViewsContainer;
        public PlayPremiumLogView itemViewPrefab;
        public GameObject loading;
        public GameObject error;
        public GameObject empty;
        public Button upButton;

        private PlayPremiumLogGroupViewModel _viewModel;
        private GameObject _popups;
        private AsyncLazy<PlayPremiumLogDetailView> _detailViewPrefab;

        public void Setup(PlayPremiumLogGroupViewModel viewModel, GameObject popups)
        {
            _viewModel = viewModel;
            _popups = popups;
            _detailViewPrefab = new(() => ResourcesUtility.LoadViewAsync<PlayPremiumLogDetailView>(destroyCancellationToken));
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        private void Start()
        {
            var openDetail = new ReactiveCommand<PlayPremiumLogViewModel>().AddTo(this);
            openDetail.SubscribeAwait(
                async (x, ct) => _popups.AddSingle(await _detailViewPrefab).Setup(x),
                AwaitOperation.Drop);

            _viewModel.Value
                .Subscribe(x =>
                {
                    itemViewsContainer.Children().Destroy();

                    if (x is not null)
                    {
                        foreach (var item in x)
                        {
                            itemViewsContainer.Add(itemViewPrefab).Setup(item, openDetail);
                        }
                    }
                })
                .AddTo(this);

            _viewModel.FetchState.Select(x => x.IsRunning).SubscribeToActive(loading).AddTo(this);

            _viewModel.FetchState.Select(x => x.IsFaulted).SubscribeToActive(error).AddTo(this);
            _viewModel.FetchState.Where(x => x.IsFaulted).Subscribe(x => Debug.Log($"Failed to fetch premium logs. {x.Exception}")).AddTo(this);

            _viewModel.FetchState
                .CombineLatest(
                    _viewModel.Value,
                    (fetchState, value) => fetchState.IsCompletedSuccessfully && (value is null || !value.Any()))
                .SubscribeToActive(empty)
                .AddTo(this);

            upButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);

            _viewModel.Fetch.Execute(Unit.Default);
        }
    }
}
