using Cysharp.Threading.Tasks;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameResultSpecialtyView : MonoBehaviour
    {
        public Image iconImage;
        public TMP_Text nameText;
        public TMP_Text numberText;

        private GameResultSpecialty _viewModel;

        public void Setup(GameResultSpecialty viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            LoadIconImageAsync().Forget();
            _viewModel.Id.GetName().AsRStringObservable().SubscribeToText(nameText).AddTo(this);
            R.Culture.Select(_ => StringFormatter.FormatCount(_viewModel.Number)).SubscribeToText(numberText).AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync()
        {
            iconImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(_viewModel.Id.GetIconPath(), destroyCancellationToken);
        }
    }
}
