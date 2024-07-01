using System;
using System.Threading.Tasks;
using R3;
using Some1.Prefs.Data;
using Some1.Prefs.Front;

namespace Some1.Prefs.UI
{
    public sealed class PrefsViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PrefsViewModel(IPrefsFront front)
        {
            Culture = front.Culture.Connect().AddTo(_disposables);
            Theme = front.Theme.Connect().AddTo(_disposables);
            MusicVolume = front.MusicVolume.Connect().AddTo(_disposables);
            SoundVolume = front.SoundVolume.Connect().AddTo(_disposables);
            UISize = front.UISize.Connect().AddTo(_disposables);
            CameraMode = front.CameraMode.Connect().AddTo(_disposables);
            GraphicQuality = front.GraphicQuality.Connect().AddTo(_disposables);
            Fps = front.Fps.Connect().AddTo(_disposables);
            WalkJoystickHold = front.WalkJoystickHold.Connect().AddTo(_disposables);
            JitterBuffer = front.JitterBuffer.Connect().AddTo(_disposables);

            FetchAll = new ReactiveCommand<Unit>().AddTo(_disposables);
            SetCulture = new ReactiveCommand<Culture>().AddTo(_disposables);
            SetTheme = new ReactiveCommand<Theme>().AddTo(_disposables);
            SetMusicVolume = new ReactiveCommand<MusicVolume>().AddTo(_disposables);
            SetSoundVolume = new ReactiveCommand<SoundVolume>().AddTo(_disposables);
            SetUISize = new ReactiveCommand<UISize>().AddTo(_disposables);
            SetCameraMode = new ReactiveCommand<CameraMode>().AddTo(_disposables);
            SetGraphicQuality = new ReactiveCommand<GraphicQuality>().AddTo(_disposables);
            SetFps = new ReactiveCommand<Fps>().AddTo(_disposables);
            SetWalkJoystickHold = new ReactiveCommand<WalkJoystickHold>().AddTo(_disposables);
            SetJitterBuffer = new ReactiveCommand<JitterBuffer>().AddTo(_disposables);
            ResetAllExceptCulture = new ReactiveCommand<Unit>().AddTo(_disposables);

            FetchAll.SubscribeAwait(
                async (x, ct) =>
                {
                    await Task.WhenAll(
                        front.FetchCultureAsync(ct),
                        front.FetchThemeAsync(ct),
                        front.FetchMusicVolumeAsync(ct),
                        front.FetchSoundVolumeAsync(ct),
                        front.FetchUISizeAsync(ct),
                        front.FetchCameraModeAsync(ct),
                        front.FetchFpsAsync(ct),
                        front.FetchGraphicQualityAsync(ct),
                        front.FetchWalkJoystickHoldAsync(ct),
                        front.FetchJitterBufferAsync(ct));
                },
                AwaitOperation.Drop);

            SetCulture.SubscribeAwait(async (x, ct) => await front.SetCultureAsync(x, ct), AwaitOperation.Drop);
            SetTheme.SubscribeAwait(async (x, ct) => await front.SetThemeAsync(x, ct), AwaitOperation.Drop);
            SetMusicVolume.SubscribeAwait(async (x, ct) => await front.SetMusicVolumeAsync(x, ct), AwaitOperation.Drop);
            SetSoundVolume.SubscribeAwait(async (x, ct) => await front.SetSoundVolumeAsync(x, ct), AwaitOperation.Drop);
            SetUISize.SubscribeAwait(async (x, ct) => await front.SetUISizeAsync(x, ct), AwaitOperation.Drop);
            SetCameraMode.SubscribeAwait(async (x, ct) => await front.SetCameraModeAsync(x, ct), AwaitOperation.Drop);
            SetGraphicQuality.SubscribeAwait(async (x, ct) => await front.SetGraphicQualityAsync(x, ct), AwaitOperation.Drop);
            SetFps.SubscribeAwait(async (x, ct) => await front.SetFpsAsync(x, ct), AwaitOperation.Drop);
            SetWalkJoystickHold.SubscribeAwait(async (x, ct) => await front.SetWalkJoystickHoldAsync(x, ct), AwaitOperation.Drop);
            SetJitterBuffer.SubscribeAwait(async (x, ct) => await front.SetJitterBufferAsync(x, ct), AwaitOperation.Drop);
            ResetAllExceptCulture.SubscribeAwait(async (x, ct) => await front.ResetAllExceptCultureAsync(ct));
        }

        public ReadOnlyReactiveProperty<Culture> Culture { get; }

        public ReadOnlyReactiveProperty<Theme> Theme { get; }

        public ReadOnlyReactiveProperty<MusicVolume> MusicVolume { get; }

        public ReadOnlyReactiveProperty<SoundVolume> SoundVolume { get; }

        public ReadOnlyReactiveProperty<UISize> UISize { get; }

        public ReadOnlyReactiveProperty<CameraMode> CameraMode { get; }

        public ReadOnlyReactiveProperty<GraphicQuality> GraphicQuality { get; }

        public ReadOnlyReactiveProperty<Fps> Fps { get; }

        public ReadOnlyReactiveProperty<WalkJoystickHold> WalkJoystickHold { get; }

        public ReadOnlyReactiveProperty<JitterBuffer> JitterBuffer { get; }

        public ReactiveCommand<Unit> FetchAll { get; }

        public ReactiveCommand<Culture> SetCulture { get; }

        public ReactiveCommand<Theme> SetTheme { get; }

        public ReactiveCommand<MusicVolume> SetMusicVolume { get; }

        public ReactiveCommand<SoundVolume> SetSoundVolume { get; }

        public ReactiveCommand<UISize> SetUISize { get; }

        public ReactiveCommand<CameraMode> SetCameraMode { get; }
        
        public ReactiveCommand<GraphicQuality> SetGraphicQuality { get; }

        public ReactiveCommand<Fps> SetFps { get; }

        public ReactiveCommand<WalkJoystickHold> SetWalkJoystickHold { get; }

        public ReactiveCommand<JitterBuffer> SetJitterBuffer { get; }

        public ReactiveCommand<Unit> ResetAllExceptCulture { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
