using Cysharp.Threading.Tasks;
using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Some1.User.Unity.Components;

namespace Some1.User.Unity.Views
{
    public class PlayPageEmojiView : MonoBehaviour
    {
        public Button button;
        public PlayPopupsView popups;
        public Delay delay;

        private PlayPageEmojiViewModel _viewModel;

        public void Setup(PlayPageEmojiViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.NormalizedDelay.Subscribe(x => delay.NormalizedValue = x).AddTo(this);

            button.BindToOnClick(_ => popups.OpenOrCloseEmojisAsync(destroyCancellationToken)).AddTo(this);
        }
    }
}
