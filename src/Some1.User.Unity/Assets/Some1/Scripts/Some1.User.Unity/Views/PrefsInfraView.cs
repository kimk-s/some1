using System;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Info;
using Some1.Prefs.UI;
using Some1.User.Unity.Utilities;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PrefsInfraView : MonoBehaviour
    {
        public RectTransform[] letterBox;
        public Joystick walkJoystick;

        private PrefsViewModel _viewModel;
        private ReactiveProperty<Resolution> _screenResolution;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PrefsViewModel>();
        }

        private void Update()
        {
            _screenResolution.Value = Screen.currentResolution;
        }

        private static float GetAspectRatio(Resolution x) => x.height == 0 ? 0 : (float)x.width / x.height;

        private void Start()
        {
            _screenResolution = new ReactiveProperty<Resolution>(Screen.currentResolution).AddTo(this);

            _screenResolution.Subscribe(x =>
            {
                float screenAspectRatio = GetAspectRatio(x);
                GlobalBinding.Instance.CanvasScaler.matchWidthOrHeight = screenAspectRatio < PlayConst.StandardAspectRatio ? 0 : 1;
            });

            _viewModel.UISize
                .Subscribe(x => GlobalBinding.Instance.CanvasScaler.referenceResolution = x.GetCanvasResolution().ToUnityVector2())
                .AddTo(this);

            _viewModel.CameraMode
                .CombineLatest(
                    _screenResolution,
                    (cameraMode, screenResolution) => (cameraMode, screenResolution))
                .Subscribe(x =>
                {
                    float screenAspectRatio = GetAspectRatio(x.screenResolution);
                    Rect rect = new(0, 0, 1, 1);
                    float fov = PlayConst.StandardFieldOfView;

                    if (screenAspectRatio < PlayConst.StandardAspectRatio)
                    {
                        float scale = screenAspectRatio / PlayConst.StandardAspectRatio;

                        switch (x.cameraMode)
                        {
                            case Prefs.Data.CameraMode.Fit:
                                {
                                    rect = new(
                                        rect.x,
                                        (rect.height - (rect.height * scale)) * 0.5f,
                                        rect.width,
                                        rect.height * scale);
                                }
                                break;
                            case Prefs.Data.CameraMode.Fill:
                                {
                                    fov = PlayConst.StandardFieldOfView * scale;
                                }
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        float scale = PlayConst.StandardAspectRatio / screenAspectRatio;

                        switch (x.cameraMode)
                        {
                            case Prefs.Data.CameraMode.Fit:
                                {
                                    rect = new Rect(
                                        (rect.width - (rect.width * scale)) * 0.5f,
                                        rect.y,
                                        rect.width * scale,
                                        rect.height);
                                }
                                break;
                            case Prefs.Data.CameraMode.Fill:
                                {
                                    fov = PlayConst.StandardFieldOfView * scale;
                                }
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                    }

                    UpdateCameras(GlobalBinding.Instance.Cameras, fov);
                    UpdateLetterBox(letterBox, rect);

                    static void UpdateCameras(Camera[] cameras, float fov)
                    {
                        foreach (var item in cameras)
                        {
                            item.fieldOfView = fov;
                        }
                    }

                    static void UpdateLetterBox(RectTransform[] letterBox, Rect rect)
                    {
                        {
                            var left = letterBox[0];
                            left.anchoredPosition = new Vector2(0, 0);
                            left.anchorMin = new Vector2(0, 0);
                            left.anchorMax = new Vector2(rect.xMin, 1);
                        }

                        {
                            var right = letterBox[1];
                            right.anchoredPosition = new Vector2(0, 0);
                            right.anchorMin = new Vector2(rect.xMax, 0);
                            right.anchorMax = new Vector2(1, 1);
                        }

                        {
                            var top = letterBox[2];
                            top.anchoredPosition = new Vector2(0, 0);
                            top.anchorMin = new Vector2(0, rect.yMax);
                            top.anchorMax = new Vector2(1, 1);
                        }

                        {
                            var bottom = letterBox[3];
                            bottom.anchoredPosition = new Vector2(0, 0);
                            bottom.anchorMin = new Vector2(0, 0);
                            bottom.anchorMax = new Vector2(1, rect.yMin);
                        }
                    }
                })
                .AddTo(this);

            _viewModel.GraphicQuality
                .Subscribe(x =>
                {
                    int index = Array.IndexOf(QualitySettings.names, x.ToString());

                    if (index == -1)
                    {
                        Debug.LogWarning($"Failed to GraphicQuality index of {x}");
                        return;
                    }

                    QualitySettings.SetQualityLevel(index, true);
                })
                .AddTo(this);

            _viewModel.Fps.Subscribe(x => Application.targetFrameRate = x.GetInteger()).AddTo(this);

            _viewModel.WalkJoystickHold.Subscribe(x => walkJoystick.handleFollowable = !x.GetBoolean()).AddTo(this);

            _viewModel.FetchAll.Execute(Unit.Default);
        }
    }
}
