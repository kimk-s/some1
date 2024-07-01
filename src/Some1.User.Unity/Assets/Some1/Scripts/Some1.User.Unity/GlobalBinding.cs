using System;
using Some1.Prefs.Data;
using Some1.User.Unity.Elements;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Utilities
{
    public class GlobalBinding : MonoBehaviour
    {
        private static GlobalBinding s_instance;
        [SerializeField]
        private TitleScreen _titleScreen;
        [SerializeField]
        private Camera[] _cameras;
        [SerializeField]
        private Camera _indicatorCamera;
        [SerializeField]
        private AudioListener _audioListener;
        [SerializeField]
        private GameObject _canvasLayer1;
        [SerializeField]
        private GameObject _canvasLayer2;
        [SerializeField]
        private CanvasScaler _canvasScaler;
        [SerializeField]
        private ElementPool _elementPool;

        public static GlobalBinding Instance => s_instance != null ? s_instance : throw new InvalidOperationException();

        public TitleScreen TitleScreen => _titleScreen;

        public Camera[] Cameras => _cameras;

        public Camera IndicatorCamera => _indicatorCamera;

        public AudioListener AudioListener => _audioListener;

        public GameObject CanvasLayer1 => _canvasLayer1;

        public GameObject CanvasLayer2 => _canvasLayer2;

        public CanvasScaler CanvasScaler => _canvasScaler;

        public ElementPool ElementPool => _elementPool;

        public ReactiveProperty<MusicVolume> MusicVolume { get; private set; }

        public ReactiveProperty<SoundVolume> SoundVolume { get; private set; }

        private void Awake()
        {
            MusicVolume = new ReactiveProperty<MusicVolume>().AddTo(this);
            SoundVolume = new ReactiveProperty<SoundVolume>().AddTo(this);

            MusicVolume
                .CombineLatest(
                    SoundVolume,
                    (a, b) => a == Prefs.Data.MusicVolume.On || b == Prefs.Data.SoundVolume.On)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(x => AudioListener.volume = x ? 1 : 0);

            if (s_instance != null)
            {
                throw new InvalidOperationException();
            }

            s_instance = this;
        }
    }
}
