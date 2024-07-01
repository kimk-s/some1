using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using R3.Triggers;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayCameraView : MonoBehaviour
    {
        private const float MinRefreshMagnitude = 0.0001f;
        private const float MaxLerpMagnitude = 1;

        private PlayCameraViewModel _viewModel;
        private Transform _mainCamera;
        private Vector3 _mainCameraOffset;
        private Transform[] _cameras;
        private Vector3[] _cameraOffsets;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Camera;
        }

        private void Start()
        {
            this.OnDestroyAsObservable().Subscribe(_ =>
            {
                for (int i = 0; i < _cameras.Length; i++)
                {
                    var camera = _cameras[i];
                    var originalPosition = _cameraOffsets[i];
                    if (camera)
                    {
                        camera.localPosition = originalPosition;
                    }
                }
            });

            _mainCamera = Camera.main.transform;
            _mainCameraOffset = _mainCamera.localPosition;
            _cameras = Camera.allCameras.Select(x => x.transform).ToArray();
            _cameraOffsets = _cameras.Select(x => x.localPosition).ToArray();
        }

        private void LateUpdate()
        {
            RefreshCamera();
        }

        private void RefreshCamera()
        {
            var myObjectPosition = _viewModel.Position.CurrentValue.ToUnityVector3();
            var mainCameraPosition = _mainCamera.localPosition - _mainCameraOffset;
            float magnitude = (myObjectPosition - mainCameraPosition).magnitude;
            if (magnitude < MinRefreshMagnitude)
            {
                return;
            }

            if (magnitude > MaxLerpMagnitude)
            {
                for (int i = 0; i < _cameras.Length; i++)
                {
                    var camera = _cameras[i];
                    var originalPosition = _cameraOffsets[i];
                    camera.localPosition = originalPosition + myObjectPosition;
                }
            }
            else
            {
                for (int i = 0; i < _cameras.Length; i++)
                {
                    var camera = _cameras[i];
                    var originalPosition = _cameraOffsets[i];
                    camera.localPosition = Vector3.Lerp(
                        camera.localPosition,
                        originalPosition + myObjectPosition,
                        Time.smoothDeltaTime * 10);
                }
            }
        }
    }
}
