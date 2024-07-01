using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class FpsBriefView : MonoBehaviour
    {
        public TMP_Text clientFpsText;
        public TMP_Text syncFpsText;

        private readonly FpsWatch _fpsWatch = new();
        private IServiceProvider _serviceProvider;
        private FpsViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = _serviceProvider.GetRequiredService<FpsViewModel>();
        }

        private void Start()
        {
            _viewModel.ClientFps.ThrottleLast(TimeSpan.FromSeconds(0.2f)).SubscribeToText(clientFpsText).AddTo(this);
            _viewModel.SyncFps.ThrottleLast(TimeSpan.FromSeconds(0.2f)).SubscribeToText(syncFpsText).AddTo(this);
        }

        private void Update()
        {
            _viewModel.SetClientFps.Execute(_fpsWatch.ComputeFps(Time.unscaledDeltaTime));
        }
    }
}
