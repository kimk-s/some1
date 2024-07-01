using Cysharp.Threading.Tasks;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class WaitUserView : MonoBehaviour
    {
        public WaitUserPhotoView photo;
        public TMP_Text primaryText;
        public TMP_Text secondaryText;

        private ReadOnlyReactiveProperty<WaitUserViewModel?> _viewModel;

        public void Setup(ReadOnlyReactiveProperty<WaitUserViewModel?> viewModel)
        {
            photo.Setup(viewModel);
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Select(x => x?.DisplayName).SubscribeToText(primaryText).AddTo(this);
            _viewModel.Select(x => x?.Email).SubscribeToText(secondaryText).AddTo(this);
        }
    }
}
