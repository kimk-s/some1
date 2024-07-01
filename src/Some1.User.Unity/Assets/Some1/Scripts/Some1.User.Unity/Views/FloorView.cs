using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class FloorView : MonoBehaviour
    {
        public ElementManager elementManager;

        private FloorViewModel _viewModel;

        public void Setup(FloorViewModel viewModel)
        {
            viewModel.State.Select(x => x is not null).SubscribeToActive(gameObject).AddTo(this);
            _viewModel = viewModel;
        }

        private void Start()
        {
            elementManager.Register(ApplyElementPosition);

            _viewModel.State
                .Subscribe(x =>
                {
                    elementManager.Path = x?.Type.GetElementPath();
                    ApplyElementPosition();
                })
                .AddTo(this);
        }

        private void ApplyElementPosition()
        {
            if (elementManager.Element == null || _viewModel.State == null)
            {
                return;
            }

            const float SortingY = -0.001f;
            var value = _viewModel.State.CurrentValue.Value.Area.Position.ToUnityVector3(SortingY);
            transform.localPosition = value;
            elementManager.Element.transform.localPosition = value;
        }
    }
}
