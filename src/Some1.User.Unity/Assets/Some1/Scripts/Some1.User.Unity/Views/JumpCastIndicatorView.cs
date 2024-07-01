using System;
using Some1.Play.Info;
using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class JumpCastIndicatorView : MonoBehaviour
    {
        public RectTransform handleRoot;
        public Image rectangleHandle;
        public Image circleHandle;

        private JumpCastIndicatorViewModel _viewModel;

        public void Setup(JumpCastIndicatorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Start()
        {
            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);
            _viewModel.HandleArea
                .Subscribe(x =>
                {
                    rectangleHandle.gameObject.SetActive(x.Type == AreaType.Rectangle);
                    circleHandle.gameObject.SetActive(x.Type == AreaType.Circle);

                    handleRoot.sizeDelta = x.Size.ToVector2().ToUnityVector2();
                })
                .AddTo(this);
            _viewModel.HandlePosition.Subscribe(x => handleRoot.localPosition = x.ToUnityVector3()).AddTo(this);
            _viewModel.Color
                .Subscribe(x =>
                {
                    rectangleHandle.color = x.ToUnityColor(rectangleHandle.color.a);
                    circleHandle.color = x.ToUnityColor(circleHandle.color.a);
                })
                .AddTo(this);
        }
    }
}
