using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class LikeIndicatorView : MonoBehaviour
    {
        public Canvas canvas;

        private LikeIndicatorViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Indicators.LikeIndicator;
        }

        private void Start()
        {
            canvas.worldCamera = GlobalBinding.Instance.IndicatorCamera;

            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);
            _viewModel.Position.Subscribe(x => transform.localPosition = x.ToUnityVector3()).AddTo(this);
        }
    }
}
