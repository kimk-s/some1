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
    public class PlayerCharacterSkinView : MonoBehaviour
    {
        public Image iconImage;
        public TMP_Text nameText;
        public GameObject radio;
        public GameObject radioEnabled;
        public GameObject radioSelected;
        public GameObject checkbox;
        public GameObject checkboxEnabled;
        public GameObject checkboxChecked;
        public Button button;

        private PlayerCharacterSkinViewModel _viewModel;
        private ReactiveCommand<SkinId> _click;

        public void Setup(
            PlayerCharacterSkinViewModel viewModel,
            ReactiveCommand<SkinId> click)
        {
            _viewModel = viewModel;
            _click = click;
        }

        private void Start()
        {
            LoadIconImageAsync().Forget();
            _viewModel.Id.GetName().AsRStringObservable().SubscribeToText(nameText).AddTo(this);

            _viewModel.IsRandomSkin.Select(x => !x).SubscribeToActive(radio).AddTo(this);
            _viewModel.IsUnlocked.SubscribeToActive(radioEnabled).AddTo(this);
            _viewModel.IsSelected.SubscribeToActive(radioSelected).AddTo(this);

            _viewModel.IsRandomSkin.SubscribeToActive(checkbox).AddTo(this);
            _viewModel.IsUnlocked.SubscribeToActive(checkboxEnabled).AddTo(this);
            _viewModel.IsRandomSelected.SubscribeToActive(checkboxChecked).AddTo(this);

            _click.BindTo(button, _ => _viewModel.Id.Skin).AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync()
        {
            iconImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(_viewModel.Id.GetIconPath(), destroyCancellationToken);
        }
    }
}
