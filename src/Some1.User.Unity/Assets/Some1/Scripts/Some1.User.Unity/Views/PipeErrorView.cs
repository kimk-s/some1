using Cysharp.Threading.Tasks;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PipeErrorView : MonoBehaviour, IBackable
    {
        public TMP_Text messageText;
        public Button retryButton;
        public Button quitButton;

        private PipeErrorViewModel _viewModel;

        public void Setup(PipeErrorViewModel viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        public bool Back()
        {
            return true;
        }

        private void Start()
        {
            _viewModel.Error.Select(x => x is null).SubscribeToDestroy(gameObject).AddTo(this);

            _viewModel.Error
                .Select(x => PlayErrorConverter.GetMessage(x))
                .Select(x => x is null || x.Length <= 200 ? x : $"{x[..200]}...")
                .SubscribeToText(messageText)
                .AddTo(this);

            _viewModel.Restart.BindTo(retryButton).AddTo(this);

            _viewModel.Quit.BindTo(quitButton).AddTo(this);
        }
    }
}
