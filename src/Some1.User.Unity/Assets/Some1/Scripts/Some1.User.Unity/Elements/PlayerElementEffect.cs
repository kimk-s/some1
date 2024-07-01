using Some1.User.Unity.Components;
using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class PlayerElementEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource? _audioSource;

        private void Awake()
        {
            if (_audioSource != null)
            {
                if (!TryGetComponent<AudioVolume>(out var audioVolume))
                {
                    audioVolume = gameObject.AddComponent<AudioVolume>();
                }

                audioVolume.type = AudioVolumeType.Sound;
            }
        }

        public void Play()
        {
            if (_audioSource != null)
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }
                _audioSource.Play();
            }
        }

        public void Stop()
        {
            if (_audioSource != null && _audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }
}
