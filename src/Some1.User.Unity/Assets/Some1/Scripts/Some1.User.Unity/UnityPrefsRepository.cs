using System;
using System.Threading;
using System.Threading.Tasks;
using Some1.Prefs.Data;
using UnityEngine;
using CameraMode = Some1.Prefs.Data.CameraMode;

namespace Some1.User.Unity
{
    public sealed class UnityPrefsRepository : IPrefsRepository
    {
        public Task<Culture> GetCultureAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<Culture>());
        }

        public Task<Theme> GetThemeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<Theme>());
        }

        public Task<MusicVolume> GetMusicVolumeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<MusicVolume>());
        }

        public Task<SoundVolume> GetSoundVolumeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<SoundVolume>());
        }

        public Task<UISize> GetUISizeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<UISize>());
        }

        public Task<CameraMode> GetCameraModeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<CameraMode>());
        }

        public Task<GraphicQuality> GetGraphicQualityAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<GraphicQuality>());
        }

        public Task<Fps> GetFpsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<Fps>());
        }

        public Task<WalkJoystickHold> GetWalkJoystickHoldAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<WalkJoystickHold>());
        }

        public Task<JitterBuffer> GetJitterBufferAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadEnum<JitterBuffer>());
        }

        public Task SetCultureAsync(Culture value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetThemeAsync(Theme value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetMusicVolumeAsync(MusicVolume value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetSoundVolumeAsync(SoundVolume value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetUISizeAsync(UISize value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetCameraModeAsync(CameraMode value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetGraphicQualityAsync(GraphicQuality value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetFpsAsync(Fps value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetWalkJoystickHoldAsync(WalkJoystickHold value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task SetJitterBufferAsync(JitterBuffer value, CancellationToken cancellationToken)
        {
            SaveEnum(value);
            return Task.CompletedTask;
        }

        public Task ResetAllExceptCultureAsync(CancellationToken cancellationToken)
        {
            DeleteEnum<Theme>();
            DeleteEnum<MusicVolume>();
            DeleteEnum<SoundVolume>();
            DeleteEnum<UISize>();
            DeleteEnum<CameraMode>();
            DeleteEnum<GraphicQuality>();
            DeleteEnum<Fps>();
            DeleteEnum<WalkJoystickHold>();
            DeleteEnum<JitterBuffer>();
            return Task.CompletedTask;
        }

        private static T LoadEnum<T>() where T : Enum
        {
            return (T)(object)PlayerPrefs.GetInt(GetKey<T>());
        }

        private static void SaveEnum<T>(T value) where T : Enum
        {
            PlayerPrefs.SetInt(GetKey<T>(), (int)(object)value);
        }

        private static void DeleteEnum<T>() where T : Enum
        {
            PlayerPrefs.DeleteKey(GetKey<T>());
        }

        private static string GetKey<T>() where T : Enum
        {
            return $"Prefs_{typeof(T).Name}";
        }
    }
}
