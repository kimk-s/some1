using Cysharp.Threading.Tasks;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerProductView : MonoBehaviour
    {
        public Image image;
        public TMP_Text titleText;
        public TMP_Text priceText;
        public GameObject offerGo;
        public TMP_Text offerText;
        public Button buyButton;

        private PlayerProductViewModel _viewModel;

        public void Setup(PlayerProductViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            LoadImageAsync().Forget();
            titleText.text = _viewModel.Title;
            priceText.text = _viewModel.Price;

            var offer = StoreProductIdConverter.GetOffer(_viewModel.Id);
            bool isOfferExists = offer is not null;
            offerGo.SetActive(isOfferExists);
            offerText.text = offer;

            _viewModel.Buy.BindTo(buyButton).AddTo(this);
        }

        private async UniTaskVoid LoadImageAsync()
        {
            image.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(StoreProductIdConverter.GetImagePath(_viewModel.Id), destroyCancellationToken);
        }
    }
}
