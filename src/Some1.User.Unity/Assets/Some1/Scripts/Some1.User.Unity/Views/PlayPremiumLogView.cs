using System;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayPremiumLogView : MonoBehaviour
    {
        public TMP_Text reasonText;
        public TMP_Text createdDateAgoText;
        public TMP_Text premiumChangedDaysText;
        public Button openDetailButton;
        public Color premiumChangedPlusColor;
        public Color premiumChangedMinusColor;

        private PlayPremiumLogViewModel _viewModel;
        private ReactiveCommand<PlayPremiumLogViewModel> _openDetail;

        public void Setup(PlayPremiumLogViewModel viewModel, ReactiveCommand<PlayPremiumLogViewModel> openDetail)
        {
            _viewModel = viewModel;
            _openDetail = openDetail;
        }

        private void OnEnable()
        {
            Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .Select(_ => _viewModel is null ? "" : StringFormatter.FormatAgo(DateTime.UtcNow - _viewModel.Value.CreatedDate))
                .TakeUntil(this.OnDisableAsObservable())
                .SubscribeToText(createdDateAgoText);
        }

        private void Start()
        {
            _viewModel.Value.Reason
                .GetString()
                .AsRStringObservable()
                .SubscribeToText(reasonText)
                .AddTo(this);

            premiumChangedDaysText.text = StringFormatter.FormatPremiumLogChangedDays(_viewModel.Value.PremiumChangedDays);
            premiumChangedDaysText.color = _viewModel.Value.PremiumChangedDays >= 0 ? premiumChangedPlusColor : premiumChangedMinusColor;

            _openDetail.BindTo(openDetailButton, _ => _viewModel).AddTo(this);
        }
    }
}
