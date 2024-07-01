using System;
using System.Collections.Concurrent;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class LogBriefView : MonoBehaviour
    {
        public TMP_Text infoCountText;
        public TMP_Text warningCountText;
        public TMP_Text errorCountText;
        public TMP_Text lastMessageText;

        private LogViewModel _viewModel;
        private float _lastMessageShowTime;
        private ConcurrentQueue<(int level, string message)> _queue;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<LogViewModel>();
        }

        private void Start()
        {
            _viewModel.InfoCount.SubscribeToText(infoCountText).AddTo(this);
            _viewModel.WarningCount.SubscribeToText(warningCountText).AddTo(this);
            _viewModel.ErrorCount.SubscribeToText(errorCountText).AddTo(this);
            _viewModel.LastMessage.SubscribeToText(lastMessageText).AddTo(this);
            _viewModel.LastMessage.Subscribe(_ => ShowLastMessage()).AddTo(this);

            _queue = new();
            Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;
        }

        private void Update()
        {
            while (_queue.TryDequeue(out var item))
            {
                _viewModel.AddMessage.Execute(item);
            }

            UpdateLastMessage();
        }

        private void UpdateLastMessage()
        {
            if (_lastMessageShowTime > 0)
            {
                _lastMessageShowTime -= Time.deltaTime;
                if (_lastMessageShowTime <= 0)
                {
                    HideLastMessage();
                }
            }
        }

        private void ShowLastMessage()
        {
            _lastMessageShowTime = 10;
            lastMessageText.gameObject.SetActive(true);
        }

        private void HideLastMessage()
        {
            _lastMessageShowTime = 0;
            lastMessageText.gameObject.SetActive(false);
        }

        private void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
            _queue.Enqueue((ToLevel(type), $"{condition}{(string.IsNullOrEmpty(stackTrace) ? "" : Environment.NewLine)}{stackTrace}"));
        }
        
        private static int ToLevel(LogType type) => type switch
        {
            LogType.Error => 2,
            LogType.Assert => 2,
            LogType.Warning => 1,
            LogType.Log => 0,
            LogType.Exception => 2,
            _ => throw new NotImplementedException(),
        };
    }
}
