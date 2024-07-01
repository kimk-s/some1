using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Prefs.UI;
using Some1.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PrefsBottomView : MonoBehaviour
    {
        public TMP_Text cultureText;
        public TMP_Text themeText;
        public TMP_Text musicVolumeText;
        public Button setCultureButton;
        public Button setThemeButton;
        public Button setMusicVolumeButton;

        private PrefsViewModel _viewModel;

        public void Setup(IServiceProvider services)
        {
            _viewModel = services.GetRequiredService<PrefsViewModel>();
        }

        private void Start()
        {
            _viewModel.Culture.Select(x => x.GetNativeName()).SubscribeToText(cultureText).AddTo(this);
            _viewModel.Theme.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(themeText).AddTo(this);
            _viewModel.MusicVolume.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(musicVolumeText).AddTo(this);

            _viewModel.SetCulture.BindTo(setCultureButton, _ => Shift(_viewModel.Culture.CurrentValue, Culture.en_US)).AddTo(this);
            _viewModel.SetTheme.BindTo(setThemeButton, _ => Shift(_viewModel.Theme.CurrentValue)).AddTo(this);
            _viewModel.SetMusicVolume.BindTo(setMusicVolumeButton, _ => Shift(_viewModel.MusicVolume.CurrentValue)).AddTo(this);
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
