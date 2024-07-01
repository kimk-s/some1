using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Elements;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitEmojiView : MonoBehaviour
    {
        private ObjectUnitEmojiViewModel _viewModel;
        private Func<UnitElementEmoji?> _getElement;

        public void Setup(ObjectUnitEmojiViewModel viewModel, Func<UnitElementEmoji?> getElement)
        {
            _viewModel = viewModel;
            _getElement = getElement;
        }

        public void Apply()
        {
            ApplyEmojiAsync().Forget();
        }

        private void Start()
        {
            _viewModel.Emoji.Subscribe(_ => ApplyEmojiAsync().Forget()).AddTo(this);
        }

        public async UniTaskVoid ApplyEmojiAsync()
        {
            var element = _getElement();
            if (element == null)
            {
                return;
            }

            element.gameObject.SetActive(false);
            element.spriteRenderer.sprite = null;

            var emoji = _viewModel.Emoji.CurrentValue;
            var level = _viewModel.Level.CurrentValue;

            if (emoji is null)
            {
                return;
            }

            var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(emoji.Value.GetImagePath(level), destroyCancellationToken);

            var element2 = _getElement();
            if (element2 != element || emoji != _viewModel.Emoji.CurrentValue || level != _viewModel.Level.CurrentValue)
            {
                return;
            }

            element2.gameObject.SetActive(true);
            element2.spriteRenderer.sprite = sprite;
            element2.spriteRenderer.size = element2.SpriteRendererSize;
        }
    }
}
