using Cysharp.Threading.Tasks;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerCharacterView : MonoBehaviour
    {
        public Image iconImage;
        public TMP_Text primaryText;
        public TMP_Text starText;
        public TMP_Text expText;
        public TMP_Text expTimeAgoText;
        public GameObject master;
        public GameObject todays;
        public GameObject radioEnabled;
        public GameObject radioSelected;
        public Button primaryButton;
        public Button secondaryButton;

        private PlayerCharacterViewModel _viewModel;
        private ReactiveCommand<CharacterId> _openDetail;

        public void Setup(PlayerCharacterViewModel viewModel, ReactiveCommand<CharacterId> openDetail)
        {
            _viewModel = viewModel;
            _openDetail = openDetail;
        }

        private void Start()
        {
            LoadIconImageAsync().Forget();

            _viewModel.Id.GetName().AsRStringObservable().SubscribeToText(primaryText).AddTo(this);

            _viewModel.Star
                .Select(x => StringFormatter.FormatBriefLevelWithStar(x))
                .SubscribeToText(starText)
                .AddTo(this);

            _viewModel.Star
                .Select(x => x.IsMaxLevel)
                .SubscribeToActive(master)
                .AddTo(this);

            _viewModel.Star
                .Select(x => StringFormatter.FormatBriefPointWithXP(x))
                .SubscribeToText(expText)
                .AddTo(this);

            _viewModel.ExpTimeAgo
                .Select(x => StringFormatter.FormatAgo(x))
                .SubscribeToText(expTimeAgoText)
                .AddTo(this);

            _viewModel.IsUnlocked.SubscribeToActive(radioEnabled).AddTo(this);

            _viewModel.IsPicked.SubscribeToActive(todays).AddTo(this);

            _viewModel.IsSelected.SubscribeToActive(radioSelected).AddTo(this);

            _openDetail.BindTo(primaryButton, _ => _viewModel.Id).AddTo(this);

            _viewModel.Select.BindTo(secondaryButton).AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync()
        {
            iconImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(_viewModel.Id.GetIconPath(), destroyCancellationToken);
        }
    }
}
