using System;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class CastIndicatorView : MonoBehaviour
    {
        public Canvas canvas;
        public RectTransform handle;
        public StraightCastIndicatorView straight;
        public CircularSectorCastIndicatorView circularSector;
        public JumpCastIndicatorView jump;

        private CastIndicatorViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            var viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Indicators.CastIndicator;
            straight.Setup(viewModel.Straight);
            circularSector.Setup(viewModel.CircularSector);
            jump.Setup(viewModel.Jump);

            _viewModel = viewModel;
        }

        public void Start()
        {
            canvas.worldCamera = GlobalBinding.Instance.IndicatorCamera;

            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);
            _viewModel.Position.Subscribe(x => transform.localPosition = x.ToUnityVector3()).AddTo(this);
        }
    }
}
