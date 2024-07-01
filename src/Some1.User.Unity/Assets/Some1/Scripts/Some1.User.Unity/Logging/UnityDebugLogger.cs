using System;
using Microsoft.Extensions.Logging;

namespace Some1.User.Unity.Logging
{
    public sealed class UnityDebugLogger : ILogger
    {
        private readonly string _name;

        public UnityDebugLogger(string name, Func<UnityDebugLoggerConfiguration> getCurrentConfig)
        {
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Information:
                    {
                        UnityEngine.Debug.Log(Message(logLevel, eventId, state, exception));
                        break;
                    }
                case LogLevel.Warning:
                    {
                        UnityEngine.Debug.LogWarning(Message(logLevel, eventId, state, exception));
                        break;
                    }
                case LogLevel.Error:
                case LogLevel.Critical:
                    {
                        UnityEngine.Debug.LogError(Message(logLevel, eventId, state, exception));
                        break;
                    }
                case LogLevel.None: break;
                default: throw new NotImplementedException();
            }
        }

        private static string ToString(LogLevel logLevel) => logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "errr",
            LogLevel.Critical => "crit",
            LogLevel.None => "none",
            _ => default,
        };

        private string Message<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception) =>
            $"{ToString(logLevel)}: {_name}[{eventId.Id}]{Environment.NewLine}{state}{(exception == null ? null : $"{Environment.NewLine}{exception}")}";
    }
}
