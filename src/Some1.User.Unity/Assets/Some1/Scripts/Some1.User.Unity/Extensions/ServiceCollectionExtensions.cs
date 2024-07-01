#if UNITY_EDITOR
#define MODE_DEBUG
#endif

using Microsoft.Extensions.Logging;
using Some1.Auth.Front;
using Some1.Net;
using Some1.Play.Client;
using Some1.Play.Client.Tcp;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.Play.Info.Alpha;
using Some1.Prefs.Data;
using Some1.Prefs.Front;
using Some1.Prefs.UI;
using Some1.Store.Admin;
using Some1.UI;
using Some1.User.Unity;
using Some1.User.ViewModel;
using Some1.Wait.Back;
using Some1.Wait.Back.MagicClient;
using Some1.Wait.Front;
#if MODE_DEBUG
using Some1.Data.InMemory;
using Some1.Play.Client.InMemory;
using Some1.Play.Core;
using Some1.Play.Core.Options;
using Some1.Play.Data;
using Some1.Play.Data.InMemory;
using Some1.Wait.Data;
using Some1.Wait.Data.InMemory;
#endif

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSome1(this IServiceCollection services, ProgramArgs args)
        {
#if !MODE_DEBUG
            if (args.Configuration.InMemory)
	        {
                throw new System.InvalidOperationException("Invalid InMemory");
	        }
#endif

            services
                .AddDev()
                .AddPrefs()
                .AddAuth()
                .AddWait(args.Configuration.InMemory ? null : args.Configuration.WaitServerAddress)
                .AddPlay(args.Environment, args.Configuration.InMemory)
                .AddSingleton<SharedCanExecute>()
                .AddLogging(builder =>
                {
                    builder
                        .AddFilter("Some1.Play", args.Environment == ProgramEnvironment.Development ? LogLevel.Trace : LogLevel.Information)
                        .AddUnityDebug();
                });

#if MODE_DEBUG
            if (args.Configuration.InMemory)
            {
                services
                    .AddSingleton<InMemoryRepository>()
                    .Configure<InMemoryRepositoryOptions>(x =>
                    {
                        //x.Wait = new InMemoryRepositoryOptionsWait()
                        //{
                        //    Maintenance = true
                        //};

                        x.Plays = new InMemoryRepositoryOptionsPlay[]
                        {
                            new()
                            {
                                Id = "in-memory-alpha-1",
                                Region = "InMemoryAlpha",
                                City = "InMemoryAlpha",
                                Number = 1,
                                Address = "https://0.0.0.0",
                                OpeningSoon = false,
                                Maintenance = false,
                                Busy = 0,
                            },
                            //new()
                            //{
                            //    Id = "in-memory-alpha-2",
                            //    Region = "InMemoryAlpha",
                            //    City = "InMemoryAlpha",
                            //    Number = 2,
                            //    Address = "https://0.0.0.0",
                            //    OpeningSoon = false,
                            //    Maintenance = false,
                            //    Busy = 0,
                            //},
                            //new()
                            //{
                            //    Id = "in-memory-beta-1",
                            //    Region = "InMemoryBeta",
                            //    City = "InMemoryBeta",
                            //    Number = 1,
                            //    Address = "https://0.0.0.0",
                            //    Maintenance = false,
                            //    Busy = 0,
                            //},
                        };
                    });
            }
#endif

            return services;
        }

        private static IServiceCollection AddDev(this IServiceCollection services)
        {
#if MODE_DEBUG
            services
                .AddSingleton<LogViewModel>()
                .AddSingleton<FpsViewModel>();
#endif
            return services;
        }

        private static IServiceCollection AddPrefs(this IServiceCollection services)
        {
            return services
                .AddSingleton<CultureGroupLongViewModel>()
                .AddSingleton<CultureGroupShortViewModel>()
                .AddSingleton<ThemeViewModel>()
                .AddSingleton<PrefsViewModel>()
                .AddSingleton<IPrefsFront, PrefsFront>()
                .AddSingleton<IPrefsRepository, UnityPrefsRepository>();
        }

        private static IServiceCollection AddAuth(this IServiceCollection services)
        {
            return services
                .AddScoped<AuthViewModel>()
                .AddSingleton<IAuthFront, FakeAuthFront>();
                //.AddSingleton<IAuthFront, FirebaseAuthFront>();
        }

        private static IServiceCollection AddWait(this IServiceCollection services, string? address)
        {
            services
                .AddScoped<WaitViewModel>()
                .AddSingleton<IWaitFront, WaitFront>()
                //.AddSingleton<IStore, UnityStore>();
                .AddSingleton<IStore, FakeStore>();

#if MODE_DEBUG
            if (address is null)
            {
                services
                    .AddScoped<IWaitBack, WaitBack>()
                    .AddSingleton<IWaitRepository, InMemoryWaitRepository>()
                    .AddSingleton<IStoreAdmin, FakeStoreAdmin>();
            }
            else
#endif
            {
                services
                    .AddScoped<IWaitBack, MagicClientWaitBack>()
                    .AddScoped<IMagicClientWaitBackAuth, MagicClientWaitBackAuth>()
                    .AddScoped<IWaitAddressGettable, WaitAddressGettable>()
                    .AddScoped<IGrpcChannelGettable, YetGrpcChannelGettable>()
                    .Configure<WaitAddressGettableOptions>(x =>
                    {
                        x.Address = address;
                    });
            }

            return services;
        }

        private static IServiceCollection AddPlay(this IServiceCollection services, ProgramEnvironment environment, bool inMemory)
        {
            services
                .AddScoped<PlayViewModel>()
                .AddSingleton<TalkGroupViewModel>()
                .AddScoped<IPlayFront, PlayFront>()
                .AddScoped<IPlayClientAuth, PlayClientAuth>()
                .AddSingleton<IPlayInfoRepository, AlphaPlayInfoRepository>(_ => new(environment.ToString()));

#if MODE_DEBUG
            if (inMemory)
            {
                services
                    .AddScoped<IPlayStreamer, InMemoryPlayStreamer>()
                    .AddScoped<IPlayClient, InMemoryPlayClient>()
                    .AddScoped<IPlayCore, PlayCore>()
                    .AddScoped<IClock, DateTimeClock>()
                    .AddSingleton<IPlayRepository, InMemoryPlayRepository>()
                    .Configure<PlayOptions>(o =>
                    {
                        o.Id = "in-memory-alpha-1";
                        o.Parallel = new()
                        {
                            Count = 1
                        };
                        o.Players = new()
                        {
                            Count = 1,
                            Busy = new(),
                        };
                    });
            }
            else
#endif
            {
                services
                    .AddScoped<IPlayStreamer, RemotePlayStreamer>()
                    .AddSingleton<RemotePlayStreamerJitterBuffer>()
                    .AddScoped<IPlayClient, TcpPlayClient>()
                    .AddSingleton<IPlayAddressGettable, PlayAddressGettable>()
                    .AddSingleton<DuplexPipeProcessor>();
            }

            return services;
        }
    }
}
