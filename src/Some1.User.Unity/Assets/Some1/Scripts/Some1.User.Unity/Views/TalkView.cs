using Cysharp.Threading.Tasks;
using R3;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class TalkView : MonoBehaviour
    {
        public TMP_Text nameText;
        public Button openDetailButton;

        private TalkViewModel _viewModel;
        private ReactiveCommand<TalkId> _openDetail;

        public void Setup(TalkViewModel viewModel, ReactiveCommand<TalkId> openDetail)
        {
            _viewModel = viewModel;
            _openDetail = openDetail;
        }

        private void Start()
        {
            _viewModel.Id.GetName().AsRStringObservable().SubscribeToText(nameText).AddTo(this);

            _openDetail.BindTo(openDetailButton, _ => _viewModel.Id).AddTo(this);
        }
    }
}
