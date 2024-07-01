using Cysharp.Threading.Tasks;
using Firebase;
using Google;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class AuthExecutingErrorView : MonoBehaviour, IBackable
    {
        public TMP_Text messageText;
        public Button okButton;

        private AuthExecutingErrorViewModel _viewModel;

        public void Setup(AuthExecutingErrorViewModel viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        public bool Back()
        {
            _viewModel.ClearError.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            _viewModel.Error.Select(x => x is null).SubscribeToDestroy(gameObject).AddTo(this);

            _viewModel.Error
                .Select(x => x switch
                {
                    GoogleSignIn.SignInException ex => $"GoogleSignIn.SignInException: {ex.Status}. {ex.Message}",
                    FirebaseException ex => $"FirebaseException: {(Firebase.Auth.AuthError)ex.ErrorCode}. {ex.Message}",
                    _ => x?.Message,
                })
                .Select(x => x is null || x.Length <= 200 ? x : $"{x[..200]}...")
                .SubscribeToText(messageText)
                .AddTo(this);

            _viewModel.ClearError.BindTo(okButton).AddTo(this);
        }
    }
}
