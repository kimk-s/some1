using Cysharp.Threading.Tasks;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameResultView : MonoBehaviour
    {
        public TMP_Text modeIconText;
        public TMP_Text primaryText;
        public TMP_Text secondaryText;
        public Button openDetailButton;

        private PlayerGameResultViewModel _viewModel;
        private ReactiveCommand<int> _openDetailView;

        public void Setup(PlayerGameResultViewModel viewModel, ReactiveCommand<int> openDetailView)
        {
            _viewModel = viewModel;
            _openDetailView = openDetailView;
            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);
        }

        private void Start()
        {
            _viewModel.Mode
                .Select(x => x?.GetIconString())
                .SubscribeToText(modeIconText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Success,
                    _viewModel.Mode,
                    _viewModel.Score,
                    (_, success, mode, score) => success is null || mode is null || score is null
                        ? ""
                        : StringFormatter.FormatGameResultPrimaryText(success.Value, mode.Value, score.Value))
                .SubscribeToText(primaryText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Ago,
                    (_, ago) => StringFormatter.FormatAgo(ago))
                .SubscribeToText(secondaryText)
                .AddTo(this);

            _openDetailView.BindTo(openDetailButton, _ => _viewModel.Id).AddTo(this);
        }
    }
}
