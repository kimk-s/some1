using Cysharp.Threading.Tasks;
using R3;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayUnaryView : MonoBehaviour, IBackable
    {
        public GameObject running;
        public GameObject result;
        public TMP_Text resultMessageText;
        public Button resultOkButton;

        private PlayUnaryViewModel _viewModel;

        public void Setup(PlayUnaryViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool Back()
        {
            if (_viewModel.Result.CurrentValue?.Unary is null)
            {
                return false;
            }

            _viewModel.ClearResult.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            _viewModel.IsRunning
                .SubscribeToActive(running)
                .AddTo(this);

            _viewModel.Result.Select(x => x?.Unary is not null)
                .SubscribeToActive(result)
                .AddTo(this);

            var resultMessage = R.Culture
                .CombineLatest(
                    _viewModel.Result,
                    (_, x) =>
                    {
                        string format = x is null ? "" : R.GetString(x.Value.GetFormat());

                        string param1 = x is null ? "" : x.Value.Unary?.Type switch
                        {
                            _ => R.GetString(x.Value.GetParam1())
                        };

                        return string.Format(format, param1);
                    });

            resultMessage
                .SubscribeToText(resultMessageText)
                .AddTo(this);

            resultMessage
                .Subscribe(_ => LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(resultMessageText.rectTransform))
                .AddTo(this);

            _viewModel.ClearResult.BindTo(resultOkButton).AddTo(this);
        }
    }
}
