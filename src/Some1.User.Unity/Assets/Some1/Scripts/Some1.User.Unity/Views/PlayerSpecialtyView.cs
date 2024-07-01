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
    public class PlayerSpecialtyView : MonoBehaviour
    {
        public Image iconImage;
        public TMP_Text primaryText;
        public TMP_Text starText;
        public TMP_Text numberText;
        public TMP_Text numberTimeAgoText;
        public GameObject master;
        public Button primaryButton;

        private PlayerSpecialtyViewModel _viewModel;
        private ReactiveCommand<SpecialtyId> _openDetail;

        public void Setup(PlayerSpecialtyViewModel viewModel, ReactiveCommand<SpecialtyId> openDetail)
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
                .Select(x => StringFormatter.FormatBriefPointWithEA(x))
                .SubscribeToText(numberText)
                .AddTo(this);

            _viewModel.NumberTimeAgo
                .Select(x => StringFormatter.FormatAgo(x))
                .SubscribeToText(numberTimeAgoText)
                .AddTo(this);

            _openDetail.BindTo(primaryButton, _ => _viewModel.Id).AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync()
        {
            iconImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(_viewModel.Id.GetIconPath(), destroyCancellationToken);
        }
    }
}
