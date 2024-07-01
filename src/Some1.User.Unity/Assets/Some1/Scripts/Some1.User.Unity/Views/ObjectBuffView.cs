using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectBuffView : MonoBehaviour
    {
        public ElementManager elementManager;

        private ObjectBuffViewModel _viewModel;

        public void Setup(ObjectBuffViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Transform.Position.Subscribe(_ => ApplyTransformPosition()).AddTo(this);
            _viewModel.Shift.Height.Subscribe(_ => ApplyShiftHeight()).AddTo(this);
            _viewModel.Transform.Rotation.Subscribe(_ => ApplyTransformRotation()).AddTo(this);

            elementManager.Register(() =>
            {
                ApplyTransformPosition();
                ApplyShiftHeight();
                ApplyTransformRotation();
            });

            _viewModel.Id.Subscribe(x => elementManager.Path = x?.GetElementPath()).AddTo(this);
        }

        private BuffElement? GetElement() => (BuffElement?)elementManager.Element;

        private void ApplyTransformPosition()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.transform.localPosition = _viewModel.Transform.Position.CurrentValue.ToUnityVector3();
        }

        private void ApplyShiftHeight()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.@base.localPosition = new(0, _viewModel.Shift.Height.CurrentValue, 0);
        }

        private void ApplyTransformRotation()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.@base.localRotation = _viewModel.Transform.Rotation.CurrentValue.ToUnityQuaternion();
        }
    }
}
