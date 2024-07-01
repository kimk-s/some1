using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerEmojiView : MonoBehaviour
    {
        public Image image;
        public Button button;

        private PlayerEmojiViewModel _viewModel;

        public void Setup(PlayerEmojiViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Emoji.Subscribe(_ => LoadImageAsync().Forget()).AddTo(this);

            _viewModel.Set.BindTo(button).AddTo(this);
        }

        private async UniTaskVoid LoadImageAsync()
        {
            var emoji = _viewModel.Emoji.CurrentValue;
            var level = _viewModel.Level.CurrentValue;

            if (emoji is null)
            {
                image.sprite = null;
            }
            else
            {
                image.sprite = null;

                var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(emoji.Value.GetImagePath(level), destroyCancellationToken);
                if (sprite == null || emoji != _viewModel.Emoji.CurrentValue || level != _viewModel.Level.CurrentValue)
                {
                    return;
                }

                image.sprite = sprite;
            }
        }
    }
}
