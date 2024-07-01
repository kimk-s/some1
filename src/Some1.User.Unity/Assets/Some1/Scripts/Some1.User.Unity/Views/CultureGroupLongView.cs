using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.Prefs.UI;
using Some1.User.Unity.Utilities;
using R3;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class CultureGroupLongView : MonoBehaviour
    {
        public PrefsBottomView prefsBottomView;
        public GameObject page;
        public GameObject popups;
        public GameObject isExecuting;
        public CultureLongView itemViewPrefab;
        public GameObject itemViewsContainer;

        private IServiceProvider _serviceProvider;
        private CultureGroupLongViewModel _viewModel;
        private AsyncLazy<AuthView> _authViewPrefab;
        private AsyncLazy<CultureGroupExecutingErrorView> _executingErrorViewPrefab;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = serviceProvider.GetRequiredService<CultureGroupLongViewModel>();
            _authViewPrefab = new(() => ResourcesUtility.LoadViewAsync<AuthView>(destroyCancellationToken));
            _executingErrorViewPrefab = new(() => ResourcesUtility.LoadViewAsync<CultureGroupExecutingErrorView>(destroyCancellationToken));
            prefsBottomView.Setup(serviceProvider);
            prefsBottomView.setCultureButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            GlobalBinding.Instance.TitleScreen.gameObject.SetActive(true);

            _viewModel.Destroy
                .Where(x => x)
                .Take(1)
                .SubscribeAwait(
                    async (_, ct) =>
                    {
                        GlobalBinding.Instance.CanvasLayer1.AddSingle(await _authViewPrefab).Setup(_serviceProvider);
                        Destroy(gameObject);
                    },
                    AwaitOperation.Drop)
                .AddTo(this);

            _viewModel.Page.SubscribeToActive(page).AddTo(this);

            foreach (var item in _viewModel.Items.OrderBy(x => x.Id.GetEnglishName()))
            {
                itemViewsContainer.Add(itemViewPrefab).Setup(item);
            }

            _viewModel.IsExecutingActive.SubscribeToActive(isExecuting).AddTo(this);

            _viewModel.ExecutingErrorViewOpen
                .Where(x => x)
                .SubscribeAwait(
                    async (_, ct) => popups.AddSingle(await _executingErrorViewPrefab).Setup(_viewModel.CreateCultureGroupExecutingErrorViewModel()),
                    AwaitOperation.Drop)
                .AddTo(this);
        }
    }
}
