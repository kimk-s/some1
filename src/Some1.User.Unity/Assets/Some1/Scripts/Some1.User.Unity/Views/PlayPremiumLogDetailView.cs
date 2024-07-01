using System;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayPremiumLogDetailView : MonoBehaviour, IBackable
    {
        public TMP_Text reasonText;
        public TMP_Text createdDateText;
        public TMP_Text premiumChangedDaysText;
        public TMP_Text premiumExpirationDateText;
        public TMP_Text noteText;
        public TMP_Text purchaseProductTitleText;
        public TMP_Text purchaseOrderIdText;
        public TMP_Text purchaseDateText;
        public Button upButton;

        private PlayPremiumLogViewModel _viewModel;

        public void Setup(PlayPremiumLogViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        private void Start()
        {
            _viewModel.Value.Reason
                .GetString()
                .AsRStringObservable()
                .SubscribeToText(reasonText)
                .AddTo(this);

            createdDateText.text = FormatDateTime(_viewModel.Value.CreatedDate);

            premiumChangedDaysText.text = StringFormatter.FormatPremiumLogChangedDays(_viewModel.Value.PremiumChangedDays);

            premiumExpirationDateText.text = FormatDateTime(_viewModel.Value.PremiumExpirationDate);

            noteText.text = _viewModel.Value.Note;

            purchaseProductTitleText.text = _viewModel.Title;

            purchaseOrderIdText.text = _viewModel.Value.PurchaseOrderId;

            purchaseDateText.text = FormatDateTime(_viewModel.Value.PurchaseDate);

            upButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }

        private static string FormatDateTime(DateTime? x) => x is null ? null : $"{x.Value.ToLocalTime():yyyy-MM-dd  HH:mm:ss  (zzz)}";
    }
}
