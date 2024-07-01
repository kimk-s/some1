using System;
using Some1.User.Unity.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UnityDebugLoggerExtensions
    {
        public static ILoggingBuilder AddUnityDebug(
            this ILoggingBuilder builder)
        {
            //builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, UnityDebugLoggerProvider>());

            //LoggerProviderOptions.RegisterProviderOptions
            //    <UnityDebugLoggerConfiguration, UnityDebugLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddUnityDebug(
            this ILoggingBuilder builder,
            Action<UnityDebugLoggerConfiguration> configure)
        {
            builder.AddUnityDebug();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
