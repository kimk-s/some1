using Cysharp.Threading.Tasks;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerSpecialtyDetailView : MonoBehaviour, IBackable
    {
        public Button upButton;
        public TMP_Text nameText;
        public Image iconImage;
        public TMP_Text descriptionText;
        public TMP_Text starLevelText;
        public TMP_Text starPointText;
        public TMP_Text numberTimeAgoText;
        public TMP_Text seasonText;
        public TMP_Text regionText;

        private PlayerSpecialtyDetailViewModel _viewModel;

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        public void Setup(PlayerSpecialtyDetailViewModel viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        private void Start()
        {
            upButton.OnClickAsObservable().Subscribe(_ => Back());

            _viewModel.Id.GetName().AsRStringObservable().SubscribeToText(nameText).AddTo(this);
            LoadIconImageAsync().Forget();
            _viewModel.Id.GetDescription().AsRStringObservable().SubscribeToText(descriptionText).AddTo(this);
            _viewModel.Star.Select(x => StringFormatter.FormatDetailLevel(x)).SubscribeToText(starLevelText).AddTo(this);
            _viewModel.Star.Select(x => StringFormatter.FormatDetailPoint(x)).SubscribeToText(starPointText).AddTo(this);
            _viewModel.NumberTimeAgo.Select(x => StringFormatter.FormatAgo(x)).SubscribeToText(numberTimeAgoText).AddTo(this);
            seasonText.text = _viewModel.Season.GetCode();
            regionText.text = _viewModel.Region.GetCode();
        }

        private async UniTaskVoid LoadIconImageAsync()
        {
            iconImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(_viewModel.Id.GetIconPath(), destroyCancellationToken);
        }
    }
}
