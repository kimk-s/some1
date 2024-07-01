using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class SignInWithEmailView : MonoBehaviour, IBackable
    {
        public PrefsBottomView prefsBottomView;
        public TMP_InputField emailInputField;
        public TMP_InputField passwordInputField;
        public Button signInButton;
        public Button upButton;

        private SignInWithEmailViewModel _viewModel;

        public void Setup(IServiceProvider services)
        {
            _viewModel = services.GetRequiredService<AuthViewModel>().CreateSignInWithEmailViewModel().AddTo(this);
            prefsBottomView.Setup(services);
        }

        public bool Back()
        {
            _viewModel.Back.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            LoadPrefs();

            _viewModel.Email.BindTo(emailInputField).AddTo(this);
            _viewModel.Password.BindTo(passwordInputField).AddTo(this);

            _viewModel.SignInWithEmail.BindTo(signInButton).AddTo(this);
            signInButton.OnClickAsObservable().Subscribe(_ => SavePrefs());

            _viewModel.Back.BindTo(upButton).AddTo(this);
            _viewModel.Back.Subscribe(_ => Destroy(gameObject)).AddTo(this);
        }

        private const string keyOfEmail = "SIGN_IN_EMAIL";
        private const string keyOfPassword = "SIGN_IN_PASSWORD";

        private void SavePrefs()
        {
            PlayerPrefs.SetString(keyOfEmail, _viewModel.Email.Value);
            PlayerPrefs.SetString(keyOfPassword, _viewModel.Password.Value);
        }

        private void LoadPrefs()
        {
            _viewModel.Email.Value = PlayerPrefs.GetString(keyOfEmail, "user1@guest.com");
            _viewModel.Password.Value = PlayerPrefs.GetString(keyOfPassword, "123456");
        }
    }
}
