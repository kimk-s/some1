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
    public class PlayerWelcomeView : MonoBehaviour
    {
        public GameObject root;
        public GameObject talkDetailViewContainer;
        public Button setButton;
        public int paddingBottom;

        private PlayerWelcomeViewModel _viewModel;
        private IServiceProvider _serviceProvider;
        
        private TalkDetailView _talkDetailView;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page2.Welcome;
            _serviceProvider = serviceProvider;
        }

        private void Start()
        {
            _viewModel.Active.SubscribeToActive(root).AddTo(this);

            var talkDetailViewPrefab = new AsyncLazy<TalkDetailView>(
                () => ResourcesUtility.LoadViewAsync<TalkDetailView>(destroyCancellationToken));

            _viewModel.Active
                .SubscribeAwait(
                    async (x, ct) =>
                    {
                        if (_talkDetailView != null)
                        {
                            Destroy(_talkDetailView.gameObject);
                            _talkDetailView = null;
                        }

                        if (x)
                        {
                            _talkDetailView = talkDetailViewContainer.Add(await talkDetailViewPrefab);
                            _talkDetailView.Setup(_serviceProvider.GetRequiredService<TalkGroupViewModel>().CreateDetail(TalkId.Welcome));
                            _talkDetailView.upButton.gameObject.SetActive(false);
                            _talkDetailView.messageViewsContainer.padding.bottom = paddingBottom;
                        }
                    })
                .AddTo(this);

            _viewModel.Set.BindTo(setButton).AddTo(this);
        }
    }
}
