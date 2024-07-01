using Cysharp.Threading.Tasks;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class AuthStartingErrorView : MonoBehaviour, IBackable
    {
        public TMP_Text messageText;
        public Button retryButton;

        private AuthStartingErrorViewModel _viewModel;

        public void Setup(AuthStartingErrorViewModel viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        public bool Back()
        {
            _viewModel.Start.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            _viewModel.Error.Select(x => x is null).SubscribeToDestroy(gameObject).AddTo(this);

            _viewModel.Error
                .Select(x => x?.Message)
                .Select(x => x is null || x.Length <= 200 ? x : $"{x[..200]}...")
                .SubscribeToText(messageText)
                .AddTo(this);

            _viewModel.Start.BindTo(retryButton).AddTo(this);
        }
    }
}
