using Cysharp.Threading.Tasks;
using R3;
using Some1.Resources;
using Some1.User.ViewModel;
using Some1.Wait.Front;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayBuyResultView : MonoBehaviour, IBackable
    {
        public GameObject result;
        public TMP_Text resultMessageText;
        public Button okButton;

        private PlayBuyResultViewModel _viewModel;

        public void Setup(PlayBuyResultViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool Back()
        {
            if (_viewModel.Result.CurrentValue == WaitBuyProductResult.None)
            {
                return false;
            }

            _viewModel.ClearResult.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            _viewModel.Result
                .Select(x => x != WaitBuyProductResult.None)
                .SubscribeToActive(result)
                .AddTo(this);

            _viewModel.Result
                .Select(x => x.GetMessage())
                .AsRStringObservable()
                .SubscribeToText(resultMessageText)
                .AddTo(this);

            _viewModel.ClearResult.BindTo(okButton).AddTo(this);
        }
    }
}
