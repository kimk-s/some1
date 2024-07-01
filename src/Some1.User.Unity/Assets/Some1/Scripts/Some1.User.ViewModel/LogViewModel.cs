using System;
using System.Linq;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class LogViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly ReactiveProperty<int> _infoCount;
        private readonly ReactiveProperty<int> _warningCount;
        private readonly ReactiveProperty<int> _errorCount;
        private readonly ReactiveProperty<string> _lastMessage;
        private readonly ReactiveProperty<string> _messages;

        public LogViewModel()
        {
            _infoCount = new ReactiveProperty<int>().AddTo(_disposable);
            _warningCount = new ReactiveProperty<int>().AddTo(_disposable);
            _errorCount = new ReactiveProperty<int>().AddTo(_disposable);
            _lastMessage = new ReactiveProperty<string>(string.Empty).AddTo(_disposable);
            _messages = new ReactiveProperty<string>(string.Empty).AddTo(_disposable);

            AddMessage = new ReactiveCommand<(int, string)>().AddTo(_disposable);
            AddMessage.Where(x => x.level == 0).Subscribe(_ => _infoCount.Value++).AddTo(_disposable);
            AddMessage.Where(x => x.level == 1).Subscribe(_ => _warningCount.Value++).AddTo(_disposable);
            AddMessage.Where(x => x.level == 2).Subscribe(_ => _errorCount.Value++).AddTo(_disposable);

            const int TruncateStartLength = 102_400;
            const int TruncateStopLength = TruncateStartLength / 2;
            AddMessage.Select(x => Format(x.level, x.message))
                .Subscribe(x =>
                {
                    _lastMessage.Value = x;
                    _messages.Value = $"{x}{Environment.NewLine}{(Messages.CurrentValue.Length < TruncateStartLength ? Messages.CurrentValue : $"{Messages.CurrentValue[..TruncateStopLength]}{Environment.NewLine}---- log reduced ----{Environment.NewLine}")}";
                })
                .AddTo(_disposable);

            ClearMessage = new ReactiveCommand<Unit>().AddTo(_disposable);
            ClearMessage.Subscribe(_ =>
            {
                _lastMessage.Value = string.Empty;
                _messages.Value = string.Empty;
                _infoCount.Value = 0;
                _warningCount.Value = 0;
                _errorCount.Value = 0;
            }).AddTo(_disposable);
        }

        public ReadOnlyReactiveProperty<int> InfoCount => _infoCount;

        public ReadOnlyReactiveProperty<int> WarningCount => _warningCount;

        public ReadOnlyReactiveProperty<int> ErrorCount => _errorCount;

        public ReadOnlyReactiveProperty<string> LastMessage => _lastMessage;

        public ReadOnlyReactiveProperty<string> Messages => _messages;

        public ReactiveCommand<(int level, string message)> AddMessage { get; }

        public ReactiveCommand<Unit> ClearMessage { get; }

        public void Dispose() => _disposable.Dispose();

        private static string Format(int level, string message) => level switch
        {
            0 => Format(message),
            1 => $"<color=#F6BE00>{Format(message)}</color>",
            2 => $"<color=#ffcdd2>{Format(message)}</color>",
            _ => Format(message)
        };

        private static string Format(string message)
        {
            return $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
        }
    }
}
