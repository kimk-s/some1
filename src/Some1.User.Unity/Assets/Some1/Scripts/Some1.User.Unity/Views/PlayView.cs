using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using R3;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayView : MonoBehaviour, IBackable
    {
        public GameObject letterBox;
        public PrefsInfraView prefsInfraView;
        public PlayPageView page;
        public PlayPopupsView popups;
        public PlayPage2View page2;
        public PlayPopups2View popups2;
        public PlayIndicatorsView indicators;
        public PlayCameraView cameraView;
        public ObjectGroupView objectGroupView;
        public FloorGroupView floorGroupView;

        private IServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;
        private PlayViewModel _viewModel;
        private AsyncLazy<WaitView> _waitViewPrefab;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope().AddTo(this);

            _waitViewPrefab = new(() => ResourcesUtility.LoadViewAsync<WaitView>(destroyCancellationToken));

            prefsInfraView.Setup(_serviceScope.ServiceProvider);
            page.Setup(_serviceScope.ServiceProvider);
            popups.Setup(_serviceScope.ServiceProvider);
            page2.Setup(_serviceScope.ServiceProvider);
            popups2.Setup(_serviceScope.ServiceProvider);
            indicators.Setup(_serviceScope.ServiceProvider);
            cameraView.Setup(_serviceScope.ServiceProvider);
            objectGroupView.Setup(_serviceScope.ServiceProvider);
            floorGroupView.Setup(_serviceScope.ServiceProvider);

            _serviceProvider = serviceProvider;
            _viewModel = _serviceScope.ServiceProvider.GetRequiredService<PlayViewModel>();
        }

        public bool Back() => BackUtility.Back(popups.gameObject, popups2.gameObject);

        private void Start()
        {
            _viewModel.StopResult
                .Where(x => x != PlayStopResult.None)
                .Take(1)
                .Subscribe(async _ =>
                {
                    GlobalBinding.Instance.CanvasLayer1.AddSingle(await _waitViewPrefab).Setup(_serviceProvider);
                    Destroy(gameObject);
                })
                .AddTo(this);

            _viewModel.TitleActive.SubscribeToActive(GlobalBinding.Instance.TitleScreen.gameObject).AddTo(this);

            _viewModel.LetterBoxActive.SubscribeToActive(letterBox).AddTo(this);

            _viewModel.LocalTimeScale.Subscribe(x => Time.timeScale = x).AddTo(this);

            _viewModel.StartPipe.Execute(Unit.Default);
        }

        private void Update()
        {
            UpdateKey();
            UpdatePlay();
        }

        private void UpdatePlay()
        {
            _viewModel.UpdatePipe.Execute(Time.deltaTime);
        }

        private System.Numerics.Vector2 _v;
        private void UpdateKey()
        {
            var v = System.Numerics.Vector2.Zero;
            if (Input.GetKey(KeyCode.W))
            {
                v.Y += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                v.Y -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                v.X += 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                v.X -= 1;
            }

            if (v != System.Numerics.Vector2.Zero)
            {
                v = System.Numerics.Vector2.Normalize(v);
            }

            if (v != System.Numerics.Vector2.Zero || _v != System.Numerics.Vector2.Zero)
            {
                _viewModel.Page.Joysticks.WalkJoystick.Execute(new(
                    v != System.Numerics.Vector2.Zero ? JoystickUiStateType.Down : JoystickUiStateType.Up,
                    v != System.Numerics.Vector2.Zero ? Vector2Helper.Angle(v) : Vector2Helper.Angle(_v),
                    v != System.Numerics.Vector2.Zero ? v.Length() : _v.Length(),
                    false));
            }
            _v = v;
        }
    }
}
