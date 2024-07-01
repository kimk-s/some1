using Cysharp.Threading.Tasks;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameArgsView : MonoBehaviour
    {
        public TMP_Text iconText;
        public TMP_Text nameText;
        public TMP_Text descriptionText;
        public TMP_Text isSelectedText;
        public TMP_Text scoreText;
        public GameObject rankingContainer;
        public TMP_Text rankingText;
        public Button selectButton;
        public Button openRankingsButton;

        private PlayerGameArgsViewModel _viewModel;
        private ReactiveCommand<Unit> _openRankingsView;

        public void Setup(PlayerGameArgsViewModel viewModel, ReactiveCommand<Unit> openRankingsView)
        {
            _viewModel = viewModel;
            _openRankingsView = openRankingsView;
        }

        private void Start()
        {
            iconText.text = _viewModel.Id.GetIconString();

            _viewModel.Id
                .GetName()
                .AsRStringObservable()
                .SubscribeToText(nameText)
                .AddTo(this);

            _viewModel.Id
                .GetDescription()
                .AsRStringObservable()
                .SubscribeToText(descriptionText)
                .AddTo(this);

            _viewModel.IsSelected
                .Select(x => CommonConverter.GetYesNo(x))
                .AsRStringObservable()
                .SubscribeToText(isSelectedText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Score,
                    (_, score) => StringFormatter.FormatScore(score))
                .SubscribeToText(scoreText)
                .AddTo(this);

            rankingContainer.SetActive(_viewModel.RankingActive);

            _viewModel.Ranking.Select(x => PlayRankingConverter.GetNumberFormat(x)).AsRStringObservable().
                CombineLatest(
                    _viewModel.Ranking,
                    (format, ranking) => ranking == 0 ? "-" : string.Format(format, ranking))
                .SubscribeToText(rankingText)
                .AddTo(this);

            _viewModel.Select.BindTo(selectButton).AddTo(this);
            _openRankingsView.BindTo(openRankingsButton).AddTo(this);
        }
    }
}
