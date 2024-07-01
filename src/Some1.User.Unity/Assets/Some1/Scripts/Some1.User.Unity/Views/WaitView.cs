using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class WaitView : MonoBehaviour, IBackable
    {
        public PrefsBottomView prefsBottomView;
        public WaitUserPhotoView userPhotoView;
        public WaitPlayServerSelectedView playServerSelectedView;
        public GameObject panel;
        public GameObject popups;
        public GameObject maintenance;
        public GameObject isStarting;
        public GameObject isExecuting;
        public Button openUserDetailButton;
        public Button selectAutomaticPlayChannelButton;
        public Button openPlayServerDetailButton;

        private IServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;
        private WaitViewModel _viewModel;
        private AsyncLazy<AuthView> _authViewPrefab;
        private AsyncLazy<PlayView> _playViewPrefab;
        private AsyncLazy<WaitUserDetailView> _userDetailViewPrefab;
        private AsyncLazy<WaitPlayServerDetailView> _playServerDetailViewPrefab;
        private AsyncLazy<WaitStartingErrorView> _startingErrorViewPrefab;
        private AsyncLazy<WaitExecutingErrorView> _executingErrorViewPrefab;
        private AsyncLazy<WaitPlayServerGroupView> _playServerGroupViewPrefab;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceScope = _serviceProvider.CreateScope().AddTo(this);
            _viewModel = _serviceScope.ServiceProvider.GetRequiredService<WaitViewModel>();
            _authViewPrefab = new(() =>
                ResourcesUtility.LoadViewAsync<AuthView>(destroyCancellationToken));
            _playViewPrefab = new(() =>
                ResourcesUtility.LoadViewAsync<PlayView>(destroyCancellationToken));
            _userDetailViewPrefab = new(() =>
                ResourcesUtility.LoadViewAsync<WaitUserDetailView>(destroyCancellationToken));
            _playServerDetailViewPrefab = new(() =>
                ResourcesUtility.LoadViewAsync<WaitPlayServerDetailView>(destroyCancellationToken));
            _startingErrorViewPrefab = new(() =>
                ResourcesUtility.LoadViewAsync<WaitStartingErrorView>(destroyCancellationToken));
            _executingErrorViewPrefab = new(() =>
                ResourcesUtility.LoadViewAsync<WaitExecutingErrorView>(destroyCancellationToken));
            _playServerGroupViewPrefab = new(() =>
                ResourcesUtility.LoadViewAsync<WaitPlayServerGroupView>(destroyCancellationToken));

            playServerSelectedView.Setup(_viewModel.SelectedPlayServer, _viewModel.OpenPlayServerGroup);
            prefsBottomView.Setup(_serviceScope.ServiceProvider);
            userPhotoView.Setup(_viewModel.User);
        }

        public bool Back() => BackUtility.Back(popups);

        private void Start()
        {
            GlobalBinding.Instance.TitleScreen.gameObject.SetActive(true);

            _viewModel.StopResult
                .Where(x => x != WaitStopResult.None)
                .Take(1)
                .Subscribe(async x =>
                {
                    try
                    {
                        switch (x)
                        {
                            case WaitStopResult.SignedOut:
                                {
                                    GlobalBinding.Instance.CanvasLayer1.AddSingle(await _authViewPrefab).Setup(_serviceProvider);
                                    Destroy(gameObject);
                                    break;
                                }
                            case WaitStopResult.Play:
                                {
                                    GlobalBinding.Instance.CanvasLayer1.AddSingle(await _playViewPrefab).Setup(_serviceProvider);
                                    Destroy(gameObject);
                                    break;
                                }
                            default: throw new NotImplementedException();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                        throw;
                    }
                })
                .AddTo(this);

            var maintenanceBlocking = _viewModel.Wait.Select(x => x?.Maintenance == true)
                .CombineLatest(
                    _viewModel.User.Select(x => x?.Manager == false),
                    (a, b) => a && b)
                .ToReadOnlyReactiveProperty()
                .AddTo(this);

            _viewModel.StartingState.Select(x => x.IsCompletedSuccessfully)
                .CombineLatest(
                    maintenanceBlocking.Select(x => !x),
                    _viewModel.StopResult.Select(x => x == WaitStopResult.None),
                    _viewModel.SelectedPlayServer.Select(x => x is not null),
                    (a, b, c, d) => a && b && c && d)
                .SubscribeToActive(panel)
                .AddTo(this);

            _viewModel.StartingState.Select(x => x.IsCompletedSuccessfully)
                .CombineLatest(
                    maintenanceBlocking.Select(x => !x),
                    _viewModel.StopResult.Select(x => x == WaitStopResult.None),
                    _viewModel.SelectedPlayServer.Select(x => x is null),
                    (a, b, c, d) => a && b && c && d)
                .Where(x => x)
                .Take(1)
                .SubscribeAwait(
                    async (_, ct) => popups.AddSingle(await _playServerGroupViewPrefab).Setup(_serviceScope.ServiceProvider),
                    AwaitOperation.Drop)
                .AddTo(this);

            maintenanceBlocking
                .SubscribeToActive(maintenance)
                .AddTo(this);

            _viewModel.StartingState
                .Select(x => x.IsRunning)
                .SubscribeToActive(isStarting)
                .AddTo(this);

            _viewModel.ExecutingState
                .Select(x => x.IsRunning)
                .SubscribeToActive(isExecuting)
                .AddTo(this);

            _viewModel.StartingState
                .Select(x => x.Exception)
                .Where(x => x is not null)
                .Subscribe(async _ => popups.Add(await _startingErrorViewPrefab).Setup(_viewModel.CreateWaitStartingErrorViewModel()))
                .AddTo(this);

            _viewModel.ExecutingState
                .Select(x => x.Exception)
                .Where(x => x is not null)
                .Subscribe(async _ => popups.AddSingle(await _executingErrorViewPrefab).Setup(_viewModel.CreateWaitExecutingErrorViewModel()))
                .AddTo(this);

            _viewModel.OpenUser.BindTo(openUserDetailButton).AddTo(this);
            _viewModel.OpenUser
                .SubscribeAwait(
                    _viewModel.SharedCanExecute,
                    async (_, ct) => popups.AddSingle(await _userDetailViewPrefab).Setup(_serviceScope.ServiceProvider),
                    AwaitOperation.Drop)
                .AddTo(this);

            _viewModel.OpenPlayServerGroup
                .SubscribeAwait(
                    _viewModel.SharedCanExecute,
                    async (_, ct) => popups.AddSingle(await _playServerGroupViewPrefab).Setup(_serviceScope.ServiceProvider),
                    AwaitOperation.Drop)
                .AddTo(this);

            _viewModel.SelectAutomaticPlayChannel.BindTo(selectAutomaticPlayChannelButton).AddTo(this);

            _viewModel.OpenPlayServerDetail.BindTo(openPlayServerDetailButton).AddTo(this);
            _viewModel.OpenPlayServerDetail
                .SubscribeAwait(
                    _viewModel.SharedCanExecute,
                    async (_, ct) => popups.AddSingle(await _playServerDetailViewPrefab).Setup(_serviceScope.ServiceProvider),
                    AwaitOperation.Drop)
                .AddTo(this);

            _viewModel.Start.Execute(Unit.Default);
        }
    }
}
