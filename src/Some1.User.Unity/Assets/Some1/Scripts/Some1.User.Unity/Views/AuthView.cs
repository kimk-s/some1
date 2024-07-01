#if UNITY_EDITOR
#define MODE_DEBUG
#endif

using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using R3.Triggers;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class AuthView : MonoBehaviour, IBackable
    {
        public PrefsBottomView prefsBottomView;
        public GameObject page;
        public GameObject panel;
        public GameObject popups;
        public GameObject isExecuting;
        public Button signInWithGoogleButton;
        public Button signInWithEmailButton;
        public Button signInButton;

        private IServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;
        private AuthViewModel _viewModel;
        private AsyncLazy<WaitView> _waitViewPrefab;
        private AsyncLazy<SignInWithEmailView> _signInWithEmailViewPrefab;
        private AsyncLazy<AuthStartingErrorView> _startingErrorViewPrefab;
        private AsyncLazy<AuthExecutingErrorView> _executingErrorViewPrefab;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.CreateScope().AddTo(this);
            _viewModel = _serviceScope.ServiceProvider.GetRequiredService<AuthViewModel>();
            _waitViewPrefab = new(() => ResourcesUtility.LoadViewAsync<WaitView>(destroyCancellationToken));
            _signInWithEmailViewPrefab = new(() => ResourcesUtility.LoadViewAsync<SignInWithEmailView>(destroyCancellationToken));
            _startingErrorViewPrefab = new(() => ResourcesUtility.LoadViewAsync<AuthStartingErrorView>(destroyCancellationToken));
            _executingErrorViewPrefab = new(() => ResourcesUtility.LoadViewAsync<AuthExecutingErrorView>(destroyCancellationToken));
            prefsBottomView.Setup(_serviceScope.ServiceProvider);
        }

        public bool Back() => BackUtility.Back(popups);

#if !MODE_DEBUG
        private void Awake()
        {
            signInWithEmailButton.gameObject.SetActive(false);
        }
#endif

        private void Start()
        {
            GlobalBinding.Instance.TitleScreen.gameObject.SetActive(true);

            _viewModel.Destroy
                .Where(x => x)
                .Take(1)
                .SubscribeAwait(
                    async (_, ct) =>
                    {
                        GlobalBinding.Instance.CanvasLayer1.AddSingle(await _waitViewPrefab).Setup(_serviceProvider);
                        Destroy(gameObject);
                    },
                    AwaitOperation.Drop)
                .AddTo(this);

            _viewModel.PageActive.SubscribeToActive(page).AddTo(this);
            _viewModel.IsExecutingActive.SubscribeToActive(isExecuting).AddTo(this);

            _viewModel.StartingError
                .Where(x => x is not null)
                .SubscribeAwait(
                    async (_, ct) => popups.Add(await _startingErrorViewPrefab).Setup(_viewModel.CreateAuthStartingErrorViewModel()),
                    AwaitOperation.Drop)
                .AddTo(this);
            _viewModel.ExecutingError
                .Where(x => x is not null)
                .SubscribeAwait(
                    async (_, ct) => popups.AddSingle(await _executingErrorViewPrefab).Setup(_viewModel.CreateAuthExecutingErrorViewModel()),
                    AwaitOperation.Drop)
                .AddTo(this);

            _viewModel.StartingError
                .Merge(_viewModel.ExecutingError)
                .Where(x => x is not null)
                .Subscribe(x => Debug.Log(x))
                .AddTo(this);

            _viewModel.SignInWithGoogle.BindTo(signInWithGoogleButton).AddTo(this);

#if MODE_DEBUG
            _viewModel.OpenSignInWithEmail.BindTo(signInWithEmailButton).AddTo(this);
            _viewModel.OpenSignInWithEmail
                .SubscribeAwait(
                    async (_, ct) => await OpenSignInWithEmailViewAsync(),
                    AwaitOperation.Drop)
                .AddTo(this);
#endif

            int signInButtonCount = 0;
            signInButton.OnClickAsObservable()
                .SubscribeAwait(
                    async (_, ct) =>
                    {
                        if (++signInButtonCount < 10)
                        {
                            return;
                        }

                        signInButtonCount = 0;
                        await OpenSignInWithEmailViewAsync();
                    },
                    AwaitOperation.Drop);
            

            _viewModel.Start.Execute(Unit.Default);
        }

        private async Task OpenSignInWithEmailViewAsync()
        {
            panel.SetActive(false);
            var signInWithEmailView = popups.AddSingle(await _signInWithEmailViewPrefab);
            signInWithEmailView.Setup(_serviceScope.ServiceProvider);
            signInWithEmailView.OnDestroyAsObservable().TakeUntil(destroyCancellationToken).Subscribe(_ => panel.SetActive(true));
        }
    }
}
