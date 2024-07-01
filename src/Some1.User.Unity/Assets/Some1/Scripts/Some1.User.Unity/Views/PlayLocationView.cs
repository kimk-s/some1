using System;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Info;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayLocationView : MonoBehaviour
    {
        public TMP_Text regionSectionNameText;
        public RectTransform localPosition;
        public AudioSource audioSource;
        public AudioClip[] townAudioClips;
        public AudioClip[] yardAudioClips;
        public AudioClip[] fieldAudioClips;
        public AudioClip[] emptyAudioClips;
        public AudioClip[] endAudioClips;

        private PlayLocationViewModel _viewModel;
        private ReadOnlyReactiveProperty<SectionType?> _sectionType;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page.Location;
        }

        private void Start()
        {
            _viewModel.RegionSection
                .Select(x => x?.Id.GetCode() ?? "")
                .SubscribeToText(regionSectionNameText)
                .AddTo(this);

            _viewModel.RegionSection
                .Select(x => x is not null)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .SubscribeToActive(localPosition.gameObject);

            _viewModel.RegionSection
                .CombineLatest(
                    _viewModel.Position,
                    (section, position) => section is null
                        ? System.Numerics.Vector2.Zero
                        : (position - section.Area.Location.ToVector2()) / section.Area.Size.ToVector2())
                .Subscribe(x => localPosition.anchorMin = localPosition.anchorMax = x.ToUnityVector2())
                .AddTo(this);

            _sectionType = _viewModel.RegionSection
                .Select(x => x?.Type)
                .ToReadOnlyReactiveProperty()
                .AddTo(this);

            _sectionType.Subscribe(_ => PlayMusic());
        }

        private void Update()
        {
            if (!audioSource.isPlaying)
            {
                PlayMusic();
            }
        }

        private void PlayMusic()
        {
            var audioClips = _sectionType.CurrentValue switch
            {
                SectionType.Town => townAudioClips,
                SectionType.Yard => yardAudioClips,
                SectionType.Field => fieldAudioClips,
                SectionType.Empty => emptyAudioClips,
                SectionType.End => endAudioClips,
                null => null,
                _ => throw new InvalidOperationException()
            };

            var audioClip = audioClips?[RandomForUnity.Next(audioClips.Length)];

            audioSource.clip = audioClip;
            audioSource.Stop();
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }
}
