using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayPage2View : MonoBehaviour
    {
        public PlayerWelcomeView welcomeView;
        public GameObject connectingBlocker;
        public GameObject finishingBlocker;

        private PlayPage2ViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page2;
            welcomeView.Setup(serviceProvider);
        }

        private void Start()
        {
            _viewModel.ActiveConnectingBlocker.SubscribeToActive(connectingBlocker).AddTo(this);
            _viewModel.ActiveFinishingBlocker.SubscribeToActive(finishingBlocker).AddTo(this);
        }
    }
}
