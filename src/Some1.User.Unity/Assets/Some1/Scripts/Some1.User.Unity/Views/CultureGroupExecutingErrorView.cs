using Cysharp.Threading.Tasks;
using R3;
using Some1.Prefs.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class CultureGroupExecutingErrorView : MonoBehaviour, IBackable
    {
        public TMP_Text messageText;
        public Button okButton;

        private CultureGroupExecutingErrorViewModel _viewModel;

        public void Setup(CultureGroupExecutingErrorViewModel viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        public bool Back()
        {
            _viewModel.OK.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            _viewModel.Message.Select(x => x is null).SubscribeToDestroy(gameObject).AddTo(this);

            _viewModel.Message.Subscribe(x =>
            {
                if (x is not null)
                {
                    Debug.LogError(x);
                }
                messageText.text = x is null || x.Length <= 200 ? x : $"{x[..200]}...";
            }).AddTo(this);

            _viewModel.OK.BindTo(okButton).AddTo(this);
        }
    }
}
