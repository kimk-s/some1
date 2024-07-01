using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Prefs.UI;
using Some1.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PrefsView : MonoBehaviour, IBackable
    {
        public TMP_Text cultureText;
        public TMP_Text themeText;
        public TMP_Text musicVolumeText;
        public TMP_Text soundVolumeText;
        public TMP_Text uiSizeText;
        public TMP_Text cameraModeText;
        public TMP_Text graphicQualityText;
        public TMP_Text fpsText;
        public TMP_Text walkJoystickHoldText;
        public TMP_Text jitterBufferText;
        public Button setCultureButton;
        public Button setThemeButton;
        public Button setMusicVolumeButton;
        public Button setSoundVolumeButton;
        public Button setUISizeButton;
        public Button setCameraModeButton;
        public Button setGraphicQualityButton;
        public Button setFpsButton;
        public Button setWalkJoystickHoldButton;
        public Button setJitterBufferButton;
        public Button resetButton;
        public Button upButton;

        private PrefsViewModel _viewModel;

        public void Setup(IServiceProvider services)
        {
            _viewModel = services.GetRequiredService<PrefsViewModel>();
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        private void Start()
        {
            _viewModel.Culture.Select(x => x.GetNativeName()).SubscribeToText(cultureText).AddTo(this);
            _viewModel.Theme.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(themeText).AddTo(this);
            _viewModel.MusicVolume.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(musicVolumeText).AddTo(this);
            _viewModel.SoundVolume.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(soundVolumeText).AddTo(this);
            _viewModel.UISize.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(uiSizeText).AddTo(this);
            _viewModel.CameraMode.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(cameraModeText).AddTo(this);
            _viewModel.GraphicQuality.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(graphicQualityText).AddTo(this);
            _viewModel.Fps.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(fpsText).AddTo(this);
            _viewModel.WalkJoystickHold.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(walkJoystickHoldText).AddTo(this);
            _viewModel.JitterBuffer.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(jitterBufferText).AddTo(this);

            _viewModel.SetCulture.BindTo(setCultureButton, _ => Shift(_viewModel.Culture.CurrentValue, Culture.en_US)).AddTo(this);
            _viewModel.SetTheme.BindTo(setThemeButton, _ => Shift(_viewModel.Theme.CurrentValue)).AddTo(this);
            _viewModel.SetMusicVolume.BindTo(setMusicVolumeButton, _ => Shift(_viewModel.MusicVolume.CurrentValue)).AddTo(this);
            _viewModel.SetSoundVolume.BindTo(setSoundVolumeButton, _ => Shift(_viewModel.SoundVolume.CurrentValue)).AddTo(this);
            _viewModel.SetUISize.BindTo(setUISizeButton, _ => Shift(_viewModel.UISize.CurrentValue)).AddTo(this);
            _viewModel.SetCameraMode.BindTo(setCameraModeButton, _ => Shift(_viewModel.CameraMode.CurrentValue)).AddTo(this);
            _viewModel.SetGraphicQuality.BindTo(setGraphicQualityButton, _ => Shift(_viewModel.GraphicQuality.CurrentValue)).AddTo(this);
            _viewModel.SetFps.BindTo(setFpsButton, _ => Shift(_viewModel.Fps.CurrentValue)).AddTo(this);
            _viewModel.SetWalkJoystickHold.BindTo(setWalkJoystickHoldButton, _ => Shift(_viewModel.WalkJoystickHold.CurrentValue)).AddTo(this);
            _viewModel.SetJitterBuffer.BindTo(setJitterBufferButton, _ => Shift(_viewModel.JitterBuffer.CurrentValue)).AddTo(this);
            _viewModel.ResetAllExceptCulture.BindTo(resetButton).AddTo(this);

            upButton.OnClickAsObservable().Subscribe(_ => Back());
        }

        private T Shift<T>(T value, T min = default) where T : struct, Enum
        {
            var values = EnumForUnity.GetValues<T>();
            int max = values.Select(x => (int)(object)x).Max();

            int result = (int)(object)value + 1;
            if (result > max)
            {
                return min;
            }

            return (T)(object)result;
        }
    }
}
