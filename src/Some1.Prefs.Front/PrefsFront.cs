using System;
using System.Threading;
using System.Threading.Tasks;
using R3;
using Some1.Prefs.Data;

namespace Some1.Prefs.Front
{
    public sealed class PrefsFront : IPrefsFront, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly IPrefsRepository _repository;
        private readonly ReactiveProperty<Culture> _culture = new();
        private readonly ReactiveProperty<Theme> _theme = new();
        private readonly ReactiveProperty<MusicVolume> _musicVolume = new();
        private readonly ReactiveProperty<SoundVolume> _soundVolume = new();
        private readonly ReactiveProperty<UISize> _uiSize = new();
        private readonly ReactiveProperty<CameraMode> _cameraMode = new();
        private readonly ReactiveProperty<GraphicQuality> _graphicQuality = new();
        private readonly ReactiveProperty<Fps> _fps = new();
        private readonly ReactiveProperty<WalkJoystickHold> _walkJoystickHold = new();
        private readonly ReactiveProperty<JitterBuffer> _jitterBuffer = new();

        public PrefsFront(IPrefsRepository repository)
        {
            _repository = repository;
            _culture.AddTo(_disposables);
            _theme.AddTo(_disposables);
            _uiSize.AddTo(_disposables);
            _cameraMode.AddTo(_disposables);
            _graphicQuality.AddTo(_disposables);
            _fps.AddTo(_disposables);
            _walkJoystickHold.AddTo(_disposables);
            _jitterBuffer.AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<Culture> Culture => _culture;

        public ReadOnlyReactiveProperty<Theme> Theme => _theme;

        public ReadOnlyReactiveProperty<MusicVolume> MusicVolume => _musicVolume;

        public ReadOnlyReactiveProperty<SoundVolume> SoundVolume => _soundVolume;

        public ReadOnlyReactiveProperty<UISize> UISize => _uiSize;

        public ReadOnlyReactiveProperty<CameraMode> CameraMode => _cameraMode;

        public ReadOnlyReactiveProperty<GraphicQuality> GraphicQuality => _graphicQuality;

        public ReadOnlyReactiveProperty<Fps> Fps => _fps;

        public ReadOnlyReactiveProperty<WalkJoystickHold> WalkJoystickHold => _walkJoystickHold;

        public ReadOnlyReactiveProperty<JitterBuffer> JitterBuffer => _jitterBuffer;

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public async Task FetchCultureAsync(CancellationToken cancellationToken)
        {
            _culture.Value = await _repository.GetCultureAsync(cancellationToken);
        }

        public async Task FetchThemeAsync(CancellationToken cancellationToken)
        {
            _theme.Value = await _repository.GetThemeAsync(cancellationToken);
        }

        public async Task FetchMusicVolumeAsync(CancellationToken cancellationToken)
        {
            _musicVolume.Value = await _repository.GetMusicVolumeAsync(cancellationToken);
        }

        public async Task FetchSoundVolumeAsync(CancellationToken cancellationToken)
        {
            _soundVolume.Value = await _repository.GetSoundVolumeAsync(cancellationToken);
        }

        public async Task FetchUISizeAsync(CancellationToken cancellationToken)
        {
            _uiSize.Value = await _repository.GetUISizeAsync(cancellationToken);
        }

        public async Task FetchCameraModeAsync(CancellationToken cancellationToken)
        {
            _cameraMode.Value = await _repository.GetCameraModeAsync(cancellationToken);
        }

        public async Task FetchGraphicQualityAsync(CancellationToken cancellationToken)
        {
            _graphicQuality.Value = await _repository.GetGraphicQualityAsync(cancellationToken);
        }

        public async Task FetchFpsAsync(CancellationToken cancellationToken)
        {
            _fps.Value = await _repository.GetFpsAsync(cancellationToken);
        }

        public async Task FetchWalkJoystickHoldAsync(CancellationToken cancellationToken)
        {
            _walkJoystickHold.Value = await _repository.GetWalkJoystickHoldAsync(cancellationToken);
        }

        public async Task FetchJitterBufferAsync(CancellationToken cancellationToken)
        {
            _jitterBuffer.Value = await _repository.GetJitterBufferAsync(cancellationToken);
        }

        public async Task SetCultureAsync(Culture value, CancellationToken cancellationToken)
        {
            if (value == Some1.Culture.None)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            await _repository.SetCultureAsync(value, cancellationToken);
            _culture.Value = value;
        }

        public async Task SetThemeAsync(Theme value, CancellationToken cancellationToken)
        {
            await _repository.SetThemeAsync(value, cancellationToken);
            _theme.Value = value;
        }

        public async Task SetMusicVolumeAsync(MusicVolume value, CancellationToken cancellationToken)
        {
            await _repository.SetMusicVolumeAsync(value, cancellationToken);
            _musicVolume.Value = value;
        }

        public async Task SetSoundVolumeAsync(SoundVolume value, CancellationToken cancellationToken)
        {
            await _repository.SetSoundVolumeAsync(value, cancellationToken);
            _soundVolume.Value = value;
        }

        public async Task SetUISizeAsync(UISize value, CancellationToken cancellationToken)
        {
            await _repository.SetUISizeAsync(value, cancellationToken);
            _uiSize.Value = value;
        }

        public async Task SetCameraModeAsync(CameraMode value, CancellationToken cancellationToken)
        {
            await _repository.SetCameraModeAsync(value, cancellationToken);
            _cameraMode.Value = value;
        }

        public async Task SetGraphicQualityAsync(GraphicQuality value, CancellationToken cancellationToken)
        {
            await _repository.SetGraphicQualityAsync(value, cancellationToken);
            _graphicQuality.Value = value;
        }

        public async Task SetFpsAsync(Fps value, CancellationToken cancellationToken)
        {
            await _repository.SetFpsAsync(value, cancellationToken);
            _fps.Value = value;
        }

        public async Task SetWalkJoystickHoldAsync(WalkJoystickHold value, CancellationToken cancellationToken)
        {
            await _repository.SetWalkJoystickHoldAsync(value, cancellationToken);
            _walkJoystickHold.Value = value;
        }

        public async Task SetJitterBufferAsync(JitterBuffer value, CancellationToken cancellationToken)
        {
            await _repository.SetJitterBufferAsync(value, cancellationToken);
            _jitterBuffer.Value = value;
        }

        public async Task ResetAllExceptCultureAsync(CancellationToken cancellationToken)
        {
            await _repository.ResetAllExceptCultureAsync(cancellationToken);

            _theme.Value = default;
            _musicVolume.Value = default;
            _soundVolume.Value = default;
            _uiSize.Value = default;
            _cameraMode.Value = default;
            _graphicQuality.Value = default;
            _fps.Value = default;
            _walkJoystickHold.Value = default;
            _jitterBuffer.Value = default;
        }
    }
}
