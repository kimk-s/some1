#if UNITY_EDITOR
#define MODE_DEBUG
#endif

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Crashlytics;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Client;
using Some1.Play.Info;
using Some1.Prefs.Front;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.Unity.Views;
using TMPro;
using Unity.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Some1.User.Unity
{
    public class Program : MonoBehaviour
    {
        public GameObject error;
        public TMP_Text errorText;

        private ProgramArgs _args;
        
        public void Setup(ProgramArgs args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        internal FirebaseApp App { get; private set; }

        public async UniTaskVoid Start()
        {
            Application.targetFrameRate = 120;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            try
            {
                await StartCore();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                if (Application.isPlaying)
                {
                    errorText.text = ex.ToString();
                    error.SetActive(true);
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RegisterFormatters()
        {
            MagicOnionGeneratedClientInitializer.RegisterMemoryPackFormatters();
        }

        private async UniTask StartCore()
        {
            IServiceProvider services = await UniTask.RunOnThreadPool(
                () => ConfigureServices().BuildServiceProvider(),
                cancellationToken: destroyCancellationToken);

            await StartDevelopmentAsync(services);
            await StartFirebaseAsync();
            await StartPlayInfoAsync(services);
            await StartPrefsAsync(services);
            await StartCulterAsync(services);
            await StartUnityServiceAsync();
        }

        private IServiceCollection ConfigureServices() => new ServiceCollection().AddSome1(_args);

        private async Task StartDevelopmentAsync(IServiceProvider services)
        {
#if MODE_DEBUG
            var developmentBriefViewPrefab = await ResourcesUtility.LoadViewAsync<DevelopmentBriefView>(destroyCancellationToken);
            var developmentBriefView = GlobalBinding.Instance.CanvasLayer2.AddSingle(developmentBriefViewPrefab);
            developmentBriefView.Setup(services);
#endif
        }

        private async Task StartFirebaseAsync()
        {
            //var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            //if (dependencyStatus != DependencyStatus.Available)
            //{
            //    throw new Exception($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            //}

            //App = FirebaseApp.DefaultInstance;
            //Crashlytics.ReportUncaughtExceptionsAsFatal = true;

            //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

            //static void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs e)
            //{
            //    Debug.Log("Received Registration Token: " + e.Token);
            //}

            //static void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
            //{
            //    Debug.Log("Received a new message from: " + e.Message.From + $" '{e.Message.Notification?.Title}' '{e.Message.Notification?.Body}'");
            //}
        }

        private async Task StartPlayInfoAsync(IServiceProvider services)
        {
            await services.GetRequiredService<IPlayInfoRepository>().LoadAsync(destroyCancellationToken);
        }

        private async Task StartPrefsAsync(IServiceProvider services)
        {
            var prefsFront = services.GetRequiredService<IPrefsFront>();

            prefsFront.Culture.Subscribe(x => R.SetCulture(x)).AddTo(this);
            prefsFront.Theme.Subscribe(x => R.SetTheme(x)).AddTo(this);
            prefsFront.MusicVolume.Subscribe(x => GlobalBinding.Instance.MusicVolume.Value = x).AddTo(this);
            prefsFront.SoundVolume.Subscribe(x => GlobalBinding.Instance.SoundVolume.Value = x).AddTo(this);
            prefsFront.JitterBuffer.Subscribe(x =>
            {
                var jitterBuffer = services.GetService<RemotePlayStreamerJitterBuffer>();
                if (jitterBuffer is not null)
                {
                    jitterBuffer.Value = x.GetValue();
                }
            }).AddTo(this);

            await Task.WhenAll(
                prefsFront.FetchCultureAsync(destroyCancellationToken),
                prefsFront.FetchThemeAsync(destroyCancellationToken),
                prefsFront.FetchMusicVolumeAsync(destroyCancellationToken),
                prefsFront.FetchSoundVolumeAsync(destroyCancellationToken));
        }

        private async Task StartCulterAsync(IServiceProvider services)
        {
            var culterGroupLongViewPrefab = await ResourcesUtility.LoadViewAsync<CultureGroupLongView>(destroyCancellationToken);
            var culterGroupLongView = GlobalBinding.Instance.CanvasLayer1.AddSingle(culterGroupLongViewPrefab);
            culterGroupLongView.Setup(services);
        }

        private async Task StartUnityServiceAsync()
        {
            string environment = _args.Environment.ToString().ToLower();
            var options = new InitializationOptions().SetEnvironmentName(environment);
            await UnityServices.InitializeAsync(options);
        }
    }
}
