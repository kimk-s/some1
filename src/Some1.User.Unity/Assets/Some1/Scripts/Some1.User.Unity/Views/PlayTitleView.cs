using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Components;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayTitleView : MonoBehaviour
    {
        public TMP_Text playerIdText;
        public TMP_Text starCharacterText;
        public TMP_Text starSpecialtyText;
        public TMP_Text starTotalText;
        public Image likeImage;
        public TMP_Text likeLevelText;
        public TMP_Text likePointText;
        public TalkLinkButton talkLinkButton;

        private PlayTitleViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.Menu.Title;
            talkLinkButton.Setup(serviceProvider, popups);
        }

        private void Start()
        {
            _viewModel.PlayerId.SubscribeToText(playerIdText).AddTo(this);
            
            _viewModel.StarCharacter.Select(x => StringFormatter.FormatBriefPoint(x)).SubscribeToText(starCharacterText).AddTo(this);
            _viewModel.StarSpecialty.Select(x => StringFormatter.FormatBriefPoint(x)).SubscribeToText(starSpecialtyText).AddTo(this);
            _viewModel.StarGrade.Select(x => StringFormatter.FormatBriefPoint(x)).SubscribeToText(starTotalText).AddTo(this);

            _viewModel.Like.Select(x => StringFormatter.FormatDetailLevel(x)).SubscribeToText(likeLevelText).AddTo(this);
            _viewModel.Like.Subscribe(x => LoadLikeImageAsync(x).Forget()).AddTo(this);

            _viewModel.Like.Select(x => StringFormatter.FormatDetailPoint(x)).SubscribeToText(likePointText).AddTo(this);
        }

        private async UniTaskVoid LoadLikeImageAsync(Leveling x)
        {
            likeImage.sprite = null;

            var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(PlayLikeConverter.GetImagePath(checked((byte)x.Level)), destroyCancellationToken);
            if (sprite == null || x != _viewModel.Like.CurrentValue)
            {
                return;
            }

            likeImage.sprite = sprite;
        }
    }
}
