using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerGameView : MonoBehaviour
    {
        public TMP_Text iconText;
        public GameObject textConatiner;
        public TMP_Text scoreText;
        public TMP_Text timeText;
        public RectTransform timeBar;

        private PlayerGameViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page.Game;
            _viewModel.Game.Select(x => x is not null).SubscribeToActive(gameObject).AddTo(this);
        }

        private void Start()
        {
            _viewModel.Game.Select(x => x?.Mode.GetIconString()).SubscribeToText(iconText).AddTo(this);

            var textActive = _viewModel.Game.Select(x => x?.Mode == Play.Info.GameMode.Challenge);
            textActive.SubscribeToActive(textConatiner).AddTo(this);
            textActive.Subscribe(_ => LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(textConatiner)).AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Game,
                    (_, game) => StringFormatter.FormatScore(game?.Score))
                .SubscribeToText(scoreText)
                .AddTo(this);

            _viewModel.Time.Select(x => StringFormatter.FormatColonTime(x)).SubscribeToText(timeText).AddTo(this);

            _viewModel.TimeNormalized.Subscribe(x => timeBar.anchorMax = new(x, timeBar.anchorMax.y)).AddTo(this);
        }
    }
}
