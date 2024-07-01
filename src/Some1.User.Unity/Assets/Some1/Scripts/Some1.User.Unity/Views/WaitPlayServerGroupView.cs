using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.User.Unity.Components;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class WaitPlayServerGroupView : MonoBehaviour, IBackable
    {
        public PrefsBottomView prefsBottomView;
        public WaitPlayServerView itemViewPrefab;
        public GameObject itemViewsContainer;
        public Button upButton;
        public TalkLinkButton talkLinkButton;
        public GameObject popups;

        private WaitPlayServerGroupViewModel _viewModel;

        public void Setup(IServiceProvider services)
        {
            _viewModel = services.GetRequiredService<WaitViewModel>().CreateWaitPlayServerGroupViewModel().AddTo(this);
            prefsBottomView.Setup(services);
            talkLinkButton.Setup(services);
        }

        public bool Back()
        {
            if (BackUtility.Back(popups))
            {
                return true;
            }

            _viewModel.Back.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            _viewModel.SelectCompleted.SubscribeToDestroy(gameObject).AddTo(this);

            _viewModel.Items
                .Subscribe(x =>
                {
                    if (x == null)
                    {
                        throw new InvalidOperationException();
                    }

                    itemViewsContainer.Children().Destroy();

                    foreach (var item in x.OrderBy(x => StringFormatter.FormatServerName(x.Id, Culture.en_US)))
                    {
                        itemViewsContainer.Add(itemViewPrefab).Setup(item);
                    }

                    LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(itemViewsContainer);
                })
                .AddTo(this);

            _viewModel.Back.BindTo(upButton).AddTo(this);

            _viewModel.Back
                .Where(x => _viewModel.Back.CanExecute())
                .ThrottleFirst(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);

            _viewModel.IsBackable.SubscribeToActive(upButton.gameObject).AddTo(this);

            _viewModel.Fetch.Execute(Unit.Default);
        }
    }
}
