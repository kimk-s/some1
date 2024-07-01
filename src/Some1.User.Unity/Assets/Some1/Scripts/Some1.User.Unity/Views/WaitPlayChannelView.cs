using R3;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class WaitPlayChannelView : MonoBehaviour
    {
        public TMP_Text numberText;
        public TMP_Text primaryText;
        public TMP_Text secondaryText;
        public Button selectButton;

        private WaitPlayChannelViewModel _viewModel;

        public void Setup(WaitPlayChannelViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            numberText.text = _viewModel.Number.ToString();

            R.Culture
                .Select(_ => StringFormatter.FormatChannelName(_viewModel.Number))
                .SubscribeToText(primaryText)
                .AddTo(this);

            R.Culture
                .Select(_ => StringFormatter.FormatChannelStatus(_viewModel.OpeningSoon, _viewModel.Maintenance, _viewModel.Busy))
                .SubscribeToText(secondaryText)
                .AddTo(this);

            _viewModel.Select.BindTo(selectButton).AddTo(this);
        }
    }
}
