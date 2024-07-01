using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.Play.Info;
using Some1.User.ViewModel;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayJoysticksView : MonoBehaviour
    {
        public Joystick walkJoystick;
        public GameObject castJoysticks;
        public CastJoystickView attackCastJoystick;
        public CastJoystickView superCastJoystick;
        public CastButtonView ultraCastButton;

        private IServiceProvider _serviceProvider;
        private PlayJoysticksViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = _serviceProvider.GetRequiredService<PlayViewModel>().Page.Joysticks;
            attackCastJoystick.Setup(serviceProvider, CastId.Attack);
            superCastJoystick.Setup(serviceProvider, CastId.Super);
            ultraCastButton.Setup(serviceProvider, CastId.Ultra);
        }

        private void Start()
        {
            walkJoystick.maxMagnitude = 1;
            walkJoystick.OnHandleAsObservable()
                .Subscribe(x => _viewModel.WalkJoystick.Execute(x.ToJoystickUiState()))
                .AddTo(this);

            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);

            _viewModel.CastJoysticksActive.SubscribeToActive(castJoysticks).AddTo(this);

            _viewModel.CastJoysticksActive
                .Where(x => !x)
                .Subscribe(_ =>
                {
                    attackCastJoystick.joystick.Clear();
                    superCastJoystick.joystick.Clear();
                })
                .AddTo(this);
        }
    }
}
