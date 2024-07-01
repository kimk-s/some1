using System;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class LogDetailView : MonoBehaviour
    {
        public TMP_Text infoCountText;
        public TMP_Text warningCountText;
        public TMP_Text errorCountText;
        public TMP_Text messagesText;

        public Button copyButton;
        public Button clearButton;

        private IServiceProvider _serviceProvider;
        private LogViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = _serviceProvider.GetRequiredService<LogViewModel>();
        }

        private void Start()
        {
            _viewModel.InfoCount.SubscribeToText(infoCountText).AddTo(this);
            _viewModel.WarningCount.SubscribeToText(warningCountText).AddTo(this);
            _viewModel.ErrorCount.SubscribeToText(errorCountText).AddTo(this);
            _viewModel.Messages.SubscribeToText(messagesText).AddTo(this);

            copyButton
                .OnClickAsObservable()
                .Subscribe(_ => GUIUtility.systemCopyBuffer = messagesText.text)
                .AddTo(this);

            _viewModel.ClearMessage.BindTo(clearButton).AddTo(this);
        }
    }
}
