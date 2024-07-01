using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class StraightCastIndicatorView : MonoBehaviour
    {
        public RectTransform handleAnchor;
        public Image handle;

        private StraightCastIndicatorViewModel _viewModel;

        public void Setup(StraightCastIndicatorViewModel vieModel)
        {
            _viewModel = vieModel;
        }

        public void Start()
        {
            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);
            _viewModel.HandleSize.Subscribe(x => handle.rectTransform.sizeDelta = new(x.Width, x.Height)).AddTo(this);
            _viewModel.HandleOffset.Subscribe(x => handle.rectTransform.anchoredPosition = new(x, handle.rectTransform.anchoredPosition.y)).AddTo(this);
            _viewModel.HandleRotation.Subscribe(x => handleAnchor.localRotation = Quaternion.AngleAxis(360 - x, Vector3.up)).AddTo(this);
            _viewModel.Color.Subscribe(x => handle.color = x.ToUnityColor(handle.color.a)).AddTo(this);
        }
    }
}
