using Cysharp.Threading.Tasks;
using R3;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class TalkDetailView : MonoBehaviour, IBackable
    {
        public TMP_Text nameText;
        public VerticalLayoutGroup messageViewsContainer;
        public TalkMessageView messageViewPrefab;
        public Button upButton;

        private TalkDetailViewModel _viewModel;

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        public void Setup(TalkDetailViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Id.GetName().AsRStringObservable().SubscribeToText(nameText).AddTo(this);

            foreach (var item in _viewModel.Messages.Values)
            {
                messageViewsContainer.gameObject.Add(messageViewPrefab).Setup(item, _viewModel.Id);
            }

            upButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }
    }
}
