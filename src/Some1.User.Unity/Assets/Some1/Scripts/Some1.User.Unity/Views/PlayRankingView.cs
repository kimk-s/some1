using Cysharp.Threading.Tasks;
using R3;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayRankingView : MonoBehaviour
    {
        public TMP_Text numberText;
        public Image medalImage;
        public TMP_Text starText;
        public GameObject starMax;
        public TMP_Text playerIdText;
        public Image playerIdImage;
        public Image likeImage;
        public TMP_Text scoreText;
        public GameObject isMine;

        private PlayRankingViewModel _viewModel;

        public void Setup(PlayRankingViewModel viewModel)
        {
            viewModel.Active.SubscribeToActive(gameObject).AddTo(this);
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Number.SubscribeToText(numberText).AddTo(this);

            _viewModel.Medal.Subscribe(_ => LoadMedalImageAsync().Forget()).AddTo(this);

            _viewModel.Like.Subscribe(_ => LoadLikeImageAsync().Forget()).AddTo(this);

            _viewModel.PlayerId.Select(x => x.GetMark()).SubscribeToText(playerIdText).AddTo(this);
            _viewModel.PlayerId.Select(x => x.GetColor()).Subscribe(x => playerIdImage.color = x).AddTo(this);

            _viewModel.StarGrade.Select(x => x.Point).SubscribeToText(starText).AddTo(this);

            _viewModel.StarGrade.Select(x => x.IsMaxLevel).SubscribeToActive(starMax).AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Score,
                    (_, score) => StringFormatter.FormatScore(score))
                .SubscribeToText(scoreText)
                .AddTo(this);

            _viewModel.IsMine.SubscribeToActive(isMine).AddTo(this);
        }

        private async UniTaskVoid LoadMedalImageAsync()
        {
            var medal = _viewModel.Medal.CurrentValue;
            var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(medal.GetImagePath(), destroyCancellationToken);
            if (medal != _viewModel.Medal.CurrentValue)
            {
                return;
            }

            medalImage.sprite = sprite;
        }

        private async UniTaskVoid LoadLikeImageAsync()
        {
            var like = _viewModel.Like.CurrentValue;
            var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(PlayLikeConverter.GetImagePath(like), destroyCancellationToken);
            if (like != _viewModel.Like.CurrentValue)
            {
                return;
            }

            likeImage.sprite = sprite;
        }
    }
}
