using System;
using Microsoft.Extensions.DependencyInjection;
using Some1.Play.Info;
using Some1.User.ViewModel;
using R3;
using UnityEngine;
using Some1.User.Unity.Components;

namespace Some1.User.Unity.Views
{
    public class CastJoystickView : MonoBehaviour
    {
        public Joystick joystick;
        public Delay delay;
        public GameObject notAnyLoadCount;

        private CastJoystickViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider, CastId id)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page.Joysticks.CastJoysticks[id];
        }

        public void Start()
        {
            _viewModel.Info.SubscribeToActive(gameObject).AddTo(this);

            var anyLoadCount = _viewModel.Id == CastId.Attack ? Observable.Return(true) : _viewModel.AnyLoadCount;
            var delay = _viewModel.Id == CastId.Attack ? Observable.Return(0f) : _viewModel.Delay;

            _viewModel.IsAvailable
                .CombineLatest(anyLoadCount, (a, b) => a && b)
                .Subscribe(x => joystick.canvasGroup.blocksRaycasts = x)
                .AddTo(this);

            delay.Subscribe(x => this.delay.NormalizedValue = x).AddTo(this);

            anyLoadCount.Select(x => !x).SubscribeToActive(notAnyLoadCount).AddTo(this);

            joystick.clickable = true;
            joystick.vibrationOnCancel = true;
            joystick.OnHandleAsObservable()
                .Subscribe(x => _viewModel.Execute.Execute(x.ToJoystickUiState()));
        }
    }
}
