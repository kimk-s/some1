using System;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class WalkIndicatorView : MonoBehaviour
    {
        public Canvas canvas;
        public RectTransform handle;

        private WalkIndicatorViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Indicators.WalkIndicator;
        }

        public void Start()
        {
            canvas.worldCamera = GlobalBinding.Instance.IndicatorCamera;

            gameObject.SetActive(false);
            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);
            _viewModel.Position.Subscribe(x => transform.localPosition = x.ToUnityVector3()).AddTo(this);
            _viewModel.HandlePosition.Subscribe(x => handle.localPosition = x.ToUnityVector3()).AddTo(this);
        }
    }
}
