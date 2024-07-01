using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using R3;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameResultDetailView : MonoBehaviour, IBackable
    {
        public TMP_Text modeIconText;
        public TMP_Text modeNameText;
        public TMP_Text scoreText;
        public TMP_Text successText;
        public TMP_Text endTimeAgoText;
        public TMP_Text playTimeText;
        public PlayerGameResultCharacterView characterView;
        public GameObject specialtyViewsContent;
        public PlayerGameResultSpecialtyView specialtyViewPrefab;
        public Button upButton;

        private PlayerGameResultDetailViewModel _viewModel;

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        public void Setup(PlayerGameResultDetailViewModel viewModel)
        {
            characterView.Setup(viewModel.Result.Select(x => x?.Character).ToReadOnlyReactiveProperty());
            _viewModel = viewModel.AddTo(this);
        }

        private void Start()
        {
            _viewModel.Result
                .Select(x => x?.Game.Mode.GetIconString() ?? "")
                .SubscribeToText(modeIconText)
                .AddTo(this);

            _viewModel.Result
                .Select(x => x?.Game.Mode.GetName() ?? "")
                .AsRStringObservable()
                .SubscribeToText(modeNameText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Result.Select(x => x?.Game.Score),
                    (_, score) => StringFormatter.FormatScore(score ?? 0))
                .SubscribeToText(scoreText)
                .AddTo(this);

            _viewModel.Result
                .Select(x => CommonConverter.GetYesNo(x?.Success ?? false))
                .AsRStringObservable()
                .SubscribeToText(successText).AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.EndTimeAgo,
                    (_, ago) => StringFormatter.FormatAgo(ago))
                .SubscribeToText(endTimeAgoText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Result,
                    (_, x) => StringFormatter.FormatTimeSpanShort(TimeSpan.FromSeconds(x?.Cycles ?? 0)))
                .SubscribeToText(playTimeText)
                .AddTo(this);

            _viewModel.Result
                .Select(x => x?.Specialties ?? Enumerable.Empty<GameResultSpecialty>())
                .Subscribe(x =>
                {
                    specialtyViewsContent.Children().Destroy();

                    foreach (var item in x)
                    {
                        specialtyViewsContent.Add(specialtyViewPrefab).Setup(item);
                    }
                })
                .AddTo(this);

            upButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }
    }
}
