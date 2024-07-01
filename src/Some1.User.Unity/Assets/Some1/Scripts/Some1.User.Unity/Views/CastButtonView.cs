using System;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Components;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class CastButtonView : MonoBehaviour
    {
        public Button button;
        public CanvasGroup canvasGroup;
        public Delay delay;
        public GameObject notAnyLoadCount;
        public TMP_Text nextTraitText;
        public TMP_Text afterNextTraitText;

        private CastJoystickViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider, CastId id)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page.Joysticks.CastJoysticks[id];
        }

        public void Start()
        {
            _viewModel.Info.SubscribeToActive(gameObject).AddTo(this);
            _viewModel.IsAvailable.Subscribe(x => canvasGroup.blocksRaycasts = x).AddTo(this);
            _viewModel.Delay.Subscribe(x => delay.NormalizedValue = x).AddTo(this);
            _viewModel.AnyLoadCount.Select(x => !x).SubscribeToActive(notAnyLoadCount).AddTo(this);
            _viewModel.NextTrait.Select(x => x.GetIconString()).SubscribeToText(nextTraitText).AddTo(this);
            _viewModel.AfterNextTrait.Select(x => x.GetIconString()).SubscribeToText(afterNextTraitText).AddTo(this);

            button.OnClickAsObservable()
                .Subscribe(x =>
                {
                    if (!_viewModel.AnyLoadCount.CurrentValue)
                    {
                        return;
                    }

                    _viewModel.Execute.Execute(new(
                        JoystickUiStateType.Up,
                        0,
                        0,
                        false));
                });
        }
    }
}
