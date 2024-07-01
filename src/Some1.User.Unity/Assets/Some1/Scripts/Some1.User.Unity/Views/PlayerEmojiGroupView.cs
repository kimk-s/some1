using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using R3.Triggers;
using Some1.Play.Info;
using Some1.User.Unity.Components;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerEmojiGroupView : MonoBehaviour, IBackable
    {
        public PlayerEmojiView[] itemViews;
        public Delay likeDelay;

        private PlayerEmojiGroupViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.Emojis;
        }

        public bool Back()
        {
            if (!gameObject.activeSelf)
            {
                return false;
            }

            gameObject.SetActive(false);
            _viewModel.LikeUiState.Value = false;
            return true;
        }

        private void Start()
        {
            Debug.Assert(itemViews.Length == _viewModel.All.Count);

            for (int i = 0; i < _viewModel.All.Count; i++)
            {
                var itemView = itemViews[i];
                var itemViewModel = _viewModel.All[(EmojiId)i];

                itemView.Setup(itemViewModel);

                itemViewModel.Set
                    .Subscribe(_ => Back())
                    .AddTo(this);

                itemView.button.OnPointerDownAsObservable()
                    .Subscribe(_ =>
                    {
                        if (itemViewModel.Emoji.CurrentValue?.Emoji.IsLike() == true)
                        {
                            _viewModel.LikeUiState.Value = true;
                        }
                    })
                    .AddTo(this);

                itemView.button.OnPointerUpAsObservable()
                    .Subscribe(_ =>
                    {
                        if (itemViewModel.Emoji.CurrentValue?.Emoji.IsLike() == true)
                        {
                            _viewModel.LikeUiState.Value = false;
                        }
                    })
                    .AddTo(this);
            }

            _viewModel.NormalizedLikeDelay
                .Subscribe(x => likeDelay.NormalizedValue = x)
                .AddTo(this);
        }
    }
}
