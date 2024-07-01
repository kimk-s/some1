using System.Drawing;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayRegionSectionView : MonoBehaviour
    {
        public TMP_Text primaryText;
        public TMP_Text secondaryText;
        public Image image;

        private PlayRegionSectionViewModel _viewModel;
        private PointF _regionLocation;
        private float _ratio;

        public void Setup(PlayRegionSectionViewModel viewModel, PointF regionLocation, float ratio)
        {
            _viewModel = viewModel;
            _regionLocation = regionLocation;
            _ratio = ratio;
        }

        private void Start()
        {
            var rt = (RectTransform)transform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            rt.pivot = Vector2.zero;
            rt.anchoredPosition = (_viewModel.Area.Location.ToVector2() - _regionLocation.ToVector2()).ToUnityVector2() * _ratio;
            rt.sizeDelta = _viewModel.Area.Size.ToVector2().ToUnityVector2() * _ratio;

            primaryText.text = _viewModel.Id.GetCode();
            _viewModel.Type.GetName().AsRStringObservable().SubscribeToText(secondaryText).AddTo(this);
            _viewModel.Id.GetColorId().AsRColorObservable().Subscribe(x => image.color = x.ToUnityColor(image.color.a)).AddTo(this);
        }
    }
}
