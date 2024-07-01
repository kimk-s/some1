using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayLocationDetailExitPopupView : MonoBehaviour, IBackable
    {
        public Button yesButton;
        public Button noButton;

        private PlayLocationDetailViewModel _viewModel;

        public void Setup(PlayLocationDetailViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        private void Start()
        {
            _viewModel.Exit.BindTo(yesButton).AddTo(this);
            noButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }
    }
}
