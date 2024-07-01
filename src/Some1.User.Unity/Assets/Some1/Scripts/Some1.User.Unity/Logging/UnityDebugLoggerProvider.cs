using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Some1.User.Unity.Logging
{
    [ProviderAlias("UnityDebug")]
    public sealed class UnityDebugLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable _onChangeToken;
        private UnityDebugLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, UnityDebugLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);

        public UnityDebugLoggerProvider(
            IOptionsMonitor<UnityDebugLoggerConfiguration> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new UnityDebugLogger(name, GetCurrentConfig));

        private UnityDebugLoggerConfiguration GetCurrentConfig() => _currentConfig;

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken.Dispose();
        }
    }
}
