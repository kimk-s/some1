using System.Threading;
using System.Threading.Tasks;
using R3;
using Some1.Prefs.Data;

namespace Some1.Prefs.Front
{
    public interface IPrefsFront
    {
        ReadOnlyReactiveProperty<Culture> Culture { get; }
        ReadOnlyReactiveProperty<Theme> Theme { get; }
        ReadOnlyReactiveProperty<MusicVolume> MusicVolume { get; }
        ReadOnlyReactiveProperty<SoundVolume> SoundVolume { get; }
        ReadOnlyReactiveProperty<UISize> UISize { get; }
        ReadOnlyReactiveProperty<CameraMode> CameraMode { get; }
        ReadOnlyReactiveProperty<GraphicQuality> GraphicQuality { get; }
        ReadOnlyReactiveProperty<Fps> Fps { get; }
        ReadOnlyReactiveProperty<WalkJoystickHold> WalkJoystickHold { get; }
        ReadOnlyReactiveProperty<JitterBuffer> JitterBuffer { get; }

        Task FetchCultureAsync(CancellationToken cancellationToken);
        Task FetchThemeAsync(CancellationToken cancellationToken);
        Task FetchMusicVolumeAsync(CancellationToken cancellationToken);
        Task FetchSoundVolumeAsync(CancellationToken cancellationToken);
        Task FetchUISizeAsync(CancellationToken cancellationToken);
        Task FetchCameraModeAsync(CancellationToken cancellationToken);
        Task FetchFpsAsync(CancellationToken cancellationToken);
        Task FetchGraphicQualityAsync(CancellationToken cancellationToken);
        Task FetchWalkJoystickHoldAsync(CancellationToken cancellationToken);
        Task FetchJitterBufferAsync(CancellationToken cancellationToken);
        Task SetCultureAsync(Culture value, CancellationToken cancellationToken);
        Task SetThemeAsync(Theme value, CancellationToken cancellationToken);
        Task SetMusicVolumeAsync(MusicVolume value, CancellationToken cancellationToken);
        Task SetSoundVolumeAsync(SoundVolume value, CancellationToken cancellationToken);
        Task SetUISizeAsync(UISize value, CancellationToken cancellationToken);
        Task SetCameraModeAsync(CameraMode value, CancellationToken cancellationToken);
        Task SetFpsAsync(Fps value, CancellationToken cancellationToken);
        Task SetGraphicQualityAsync(GraphicQuality value, CancellationToken cancellationToken);
        Task SetWalkJoystickHoldAsync(WalkJoystickHold value, CancellationToken cancellationToken);
        Task SetJitterBufferAsync(JitterBuffer value, CancellationToken cancellationToken);
        Task ResetAllExceptCultureAsync(CancellationToken cancellationToken);
    }
}
