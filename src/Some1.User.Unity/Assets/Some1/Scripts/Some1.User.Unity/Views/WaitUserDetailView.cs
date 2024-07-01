using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class WaitUserDetailView : MonoBehaviour, IBackable
    {
        public PrefsBottomView prefsBottomView;
        public WaitUserView userView;
        public Button signOutButton;
        public Button upButton;

        private WaitUserDetailViewModel _viewModel;

        public void Setup(IServiceProvider services)
        {
            prefsBottomView.Setup(services);

            var viewModel = services.GetRequiredService<WaitViewModel>().CreateWaitUserDetailViewModel().AddTo(this);
            userView.Setup(viewModel.User);
            _viewModel = viewModel;
        }

        public bool Back()
        {
            _viewModel.Back.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            _viewModel.SignOut.BindTo(signOutButton).AddTo(this);
            _viewModel.Back.BindTo(upButton).AddTo(this);
            _viewModel.Back.Subscribe(_ => Destroy(gameObject)).AddTo(this);
        }
    }
}
