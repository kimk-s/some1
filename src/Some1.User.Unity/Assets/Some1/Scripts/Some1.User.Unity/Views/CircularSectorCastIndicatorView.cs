using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class CircularSectorCastIndicatorView : MonoBehaviour
    {
        public RectTransform handleAnchor;
        public Image handle;

        private CircularSectorCastIndicatorViewModel _viewModel;

        public void Setup(CircularSectorCastIndicatorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);
            _viewModel.HandleRadiusAndAngle
                .Subscribe(x =>
                {
                    handle.rectTransform.sizeDelta = new Vector2(x.X, x.X) * 2;
                    handle.rectTransform.localRotation = Quaternion.AngleAxis(360 - x.Y * 0.5f, Vector3.back);
                    handle.fillAmount = x.Y / 360;
                })
                .AddTo(this);
            _viewModel.HandleOffset.Subscribe(x => handle.rectTransform.anchoredPosition = new(x, handle.rectTransform.anchoredPosition.y)).AddTo(this);
            _viewModel.HandleRotation.Subscribe(x => handleAnchor.localRotation = Quaternion.AngleAxis(360 - x, Vector3.up)).AddTo(this);
            _viewModel.Color.Subscribe(x => handle.color = x.ToUnityColor(handle.color.a)).AddTo(this);
        }
    }
}
