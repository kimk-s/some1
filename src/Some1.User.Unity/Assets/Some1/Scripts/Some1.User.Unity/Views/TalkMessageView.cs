using Cysharp.Threading.Tasks;
using R3;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class TalkMessageView : MonoBehaviour
    {
        public Image senderIconImage;
        public TMP_Text senderNameText;
        public TMP_Text contentText;

        private TalkMessageViewModel _viewModel;
        private TalkId _talkId;

        public void Setup(TalkMessageViewModel viewModel, TalkId talkId)
        {
            _viewModel = viewModel;
            _talkId = talkId;
        }

        private void Start()
        {
            LoadIconImageAsync().Forget();

            _viewModel.Sender.GetName().AsRStringObservable().SubscribeToText(senderNameText).AddTo(this);

            TalkMessageConverter.GetString(_talkId, _viewModel.Id).AsRStringObservable().SubscribeToText(contentText).AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync()
        {
            senderIconImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(_viewModel.Sender.GetIconPath(), destroyCancellationToken);
        }
    }
}
