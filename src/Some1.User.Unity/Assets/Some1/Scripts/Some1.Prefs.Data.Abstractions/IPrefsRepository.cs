using System.Threading;
using System.Threading.Tasks;

namespace Some1.Prefs.Data
{
    public interface IPrefsRepository
    {
        Task<Culture> GetCultureAsync(CancellationToken cancellationToken);
        Task<Theme> GetThemeAsync(CancellationToken cancellationToken);
        Task<MusicVolume> GetMusicVolumeAsync(CancellationToken cancellationToken);
        Task<SoundVolume> GetSoundVolumeAsync(CancellationToken cancellationToken);
        Task<UISize> GetUISizeAsync(CancellationToken cancellationToken);
        Task<CameraMode> GetCameraModeAsync(CancellationToken cancellationToken);
        Task<GraphicQuality> GetGraphicQualityAsync(CancellationToken cancellationToken);
        Task<Fps> GetFpsAsync(CancellationToken cancellationToken);
        Task<WalkJoystickHold> GetWalkJoystickHoldAsync(CancellationToken cancellationToken);
        Task<JitterBuffer> GetJitterBufferAsync(CancellationToken cancellationToken);

        Task SetCultureAsync(Culture value, CancellationToken cancellationToken);
        Task SetThemeAsync(Theme value, CancellationToken cancellationToken);
        Task SetMusicVolumeAsync(MusicVolume value, CancellationToken cancellationToken);
        Task SetSoundVolumeAsync(SoundVolume value, CancellationToken cancellationToken);
        Task SetUISizeAsync(UISize value, CancellationToken cancellationToken);
        Task SetCameraModeAsync(CameraMode value, CancellationToken cancellationToken);
        Task SetGraphicQualityAsync(GraphicQuality value, CancellationToken cancellationToken);
        Task SetFpsAsync(Fps value, CancellationToken cancellationToken);
        Task SetWalkJoystickHoldAsync(WalkJoystickHold value, CancellationToken cancellationToken);
        Task SetJitterBufferAsync(JitterBuffer value, CancellationToken cancellationToken);
        Task ResetAllExceptCultureAsync(CancellationToken cancellationToken);
    }
}
