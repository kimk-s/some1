using System;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameToastView : MonoBehaviour
    {
        public GameObject container;
        public TMP_Text primaryText;
        public TMP_Text secondarayText;
        public AudioSource audioSource;
        public AudioClip readyAudioClip;
        public AudioClip returnAudioClip;
        public AudioClip reReadyAudioClip;
        public AudioClip returnFaultedAudioClip;
        public AudioClip startAudioClip;
        public AudioClip endAudioClip;
        public Button openLastResultDetailButton;

        private PlayerGameToastViewModel _viewModel;
        private PlayPopupsView _popups;

        public void Setup(IServiceProvider serviceProvider, PlayPopupsView popups)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page.GameToast;
            _popups = popups;
        }

        private void Start()
        {
            R.Culture
                .CombineLatest(
                    _viewModel.State,
                    (_, state) => (_, state))
                .Select(x => x.state.Type switch
                {
                    PlayerGameToastStateType.None => ("", ""),

                    PlayerGameToastStateType.Ready => (
                        $@"{R.GetString(x.state.GameMode?.GetName() ?? string.Empty)} {R.GetString("Play_GameToast_Ready")}
{PlayConst.PlayerGameManagerReadySeconds - x.state.Time}",
                        ""),

                    PlayerGameToastStateType.Return => (
                        $@"{R.GetString("Play_GameToast_Return")}
{PlayConst.PlayerGameManagerReturnSeconds - x.state.Time}",
                        ""),

                    PlayerGameToastStateType.ReReady => (
                        $@"{R.GetString(x.state.GameMode?.GetName() ?? string.Empty)} {R.GetString("Play_GameToast_ReReady")}
{PlayConst.PlayerGameManagerReReadySeconds - x.state.Time}",
                        ""),

                    PlayerGameToastStateType.ReturnFaulted => (
                        $@"{R.GetString("Play_GameToast_ReturnFaulted")}",
                        $@"{R.GetString("Play_GameToast_ReturnFaulted_Description")}"),

                    PlayerGameToastStateType.Start => (
                        $"{R.GetString(x.state.GameMode?.GetName() ?? string.Empty)} {R.GetString("Play_GameToast_Start")}",
                        $"{R.GetString(x.state.GameMode?.GetDescription() ?? string.Empty)}"),

                    PlayerGameToastStateType.End => (
                        @$"{R.GetString(x.state.GameMode?.GetName() ?? string.Empty)} {R.GetString("Play_GameToast_End")}",
                        @$"{StringFormatter.FormatScore(x.state.Score)}
{StringFormatter.FormatSuccessOrFailure(x.state.Success)}"),
                    _ => throw new InvalidOperationException(),
                })
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(x =>
                {
                    primaryText.text = x.Item1;
                    secondarayText.text = x.Item2;
                    LayoutRebuilderUtility.ForceRebuildBottomUpFromRoot(container);
                });

            _viewModel.State
                .Select(x => x.Type)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(x =>
                {
                    switch (x)
                    {
                        case PlayerGameToastStateType.None:
                            break;
                        case PlayerGameToastStateType.Ready:
                            Play(readyAudioClip);
                            break;
                        case PlayerGameToastStateType.Return:
                            Play(returnAudioClip);
                            break;
                        case PlayerGameToastStateType.ReReady:
                            Play(reReadyAudioClip);
                            break;
                        case PlayerGameToastStateType.ReturnFaulted:
                            Play(returnFaultedAudioClip);
                            break;
                        case PlayerGameToastStateType.Start:
                            Play(startAudioClip);
                            break;
                        case PlayerGameToastStateType.End:
                            Play(endAudioClip);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                });

            _viewModel.State.Select(x => x.Type == PlayerGameToastStateType.End)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .SubscribeToActive(openLastResultDetailButton.gameObject);

            openLastResultDetailButton.OnClickAsObservable()
                .SubscribeAwait(
                    async (_, ct) =>
                    {
                        var view = await _popups.OpenGameMenuAsync(ct);
                        view.OpenLastResultDetail();
                    },
                    AwaitOperation.Drop)
                .AddTo(this);
        }

        private void Play(AudioClip? audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
