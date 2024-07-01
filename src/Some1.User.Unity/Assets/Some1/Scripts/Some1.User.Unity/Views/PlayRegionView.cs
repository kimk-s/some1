using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayRegionView : MonoBehaviour
    {
        public GameObject sectionsContent;
        public PlayRegionSectionView sectionViewPrefab;

        private PlayRegionViewModel _viewModel;
        private float _ratio;

        public void Setup(PlayRegionViewModel viewModel, float ratio)
        {
            _viewModel = viewModel;
            _ratio = ratio;
        }

        private void Start()
        {
            var rt = (RectTransform)transform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            rt.pivot = Vector2.zero;
            rt.anchoredPosition = _viewModel.Area.Location.ToVector2().ToUnityVector2() * _ratio;
            rt.sizeDelta = _viewModel.Area.Size.ToVector2().ToUnityVector2() * _ratio;

            foreach (var item in _viewModel.Sections)
            {
                sectionsContent.Add(sectionViewPrefab).Setup(item, _viewModel.Area.Location, _ratio);
            }
        }
    }
}
