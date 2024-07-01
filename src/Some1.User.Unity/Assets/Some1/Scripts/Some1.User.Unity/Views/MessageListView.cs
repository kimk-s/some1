using Cysharp.Threading.Tasks;
using ObservableCollections;
using R3;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class MessageListView : MonoBehaviour, IBackable
    {
        public GameObject itemViewsContent;
        public MessageListItemView itemViewPrefab;

        private MessageListViewModel _viewModel;

        public void Setup(MessageListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool Back() => BackUtility.Back(itemViewsContent);

        private void Start()
        {
            _viewModel.Items.ObserveAdd()
                .Subscribe(x =>
                {
                    itemViewsContent.Add(itemViewPrefab).Setup(x.Value.Value);
                })
                .AddTo(this);
        }
    }
}
