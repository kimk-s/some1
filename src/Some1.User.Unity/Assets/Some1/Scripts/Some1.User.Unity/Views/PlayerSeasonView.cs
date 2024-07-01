using Cysharp.Threading.Tasks;
using R3;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerSeasonView : MonoBehaviour
    {
        public TMP_Text primaryText;
        public TMP_Text secondaryText;
        public GameObject comingSoon;

        private PlayerSeasonViewModel _viewModel;

        public void Setup(PlayerSeasonViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            R.Culture.Select(_ => StringFormatter.FormatSeasonCodeName(_viewModel.Id))
                .SubscribeToText(primaryText)
                .AddTo(this);
            
            secondaryText.gameObject.SetActive(_viewModel.Type != Play.Info.SeasonType.ComingSoon);
            _viewModel.Star.Select(x => x is null ? "" : $"{StringFormatter.FormatDetailPointWithStar(x.Value)}")
                .SubscribeToText(secondaryText)
                .AddTo(this);

            comingSoon.SetActive(_viewModel.Type == Play.Info.SeasonType.ComingSoon);
        }
    }
}
