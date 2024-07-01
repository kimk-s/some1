using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.Prefs.Data;
using Some1.User.Unity.Utilities;
using UnityEngine;

namespace Some1.User.Unity.Components
{
    public enum AudioVolumeType
    {
        Sound,
        Music,
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class AudioVolume : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _source;
        public AudioVolumeType type;

        private void Awake()
        {
            if (_source == null)
            {
                if (!TryGetComponent(out _source))
                {
                    throw new Exception($"Failed to get AudioSource on ({name})");
                }
            }
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                (type switch
                {
                    AudioVolumeType.Music => GlobalBinding.Instance.MusicVolume.Select(x => x == MusicVolume.On),
                    AudioVolumeType.Sound => GlobalBinding.Instance.SoundVolume.Select(x => x == SoundVolume.On),
                    _ => throw new InvalidOperationException(),
                })
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(x => _source.volume = x ? 1 : 0);
            }
        }
    }
}
