using System;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class FpsDetailView : MonoBehaviour
    {
        public TMP_Text clientFpsText;
        public TMP_Text syncFpsText;

        private IServiceProvider _serviceProvider;
        private FpsViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = _serviceProvider.GetRequiredService<FpsViewModel>();
        }

        private void Start()
        {
            _viewModel.ClientFps.SubscribeToText(clientFpsText).AddTo(this);
            _viewModel.SyncFps.SubscribeToText(syncFpsText).AddTo(this);
        }
    }
}
