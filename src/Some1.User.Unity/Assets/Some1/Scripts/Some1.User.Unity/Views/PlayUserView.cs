using System;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.User.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayUserView : MonoBehaviour, IBackable
    {
        public WaitUserView waitUserView;
        public Button upButton;

        public void Setup(IServiceProvider serviceProvider)
        {
            var viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.Menu.User;
            waitUserView.Setup(viewModel.User);
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        private void Start()
        {
            upButton.OnClickAsObservable().Subscribe(_ => Back());
        }
    }
}
