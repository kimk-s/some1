using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using Some1.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerPremiumErrorView : MonoBehaviour
    {
        public TMP_Text messageText;
        public Button okButton;

        private ReactiveProperty<TaskState> _viewModel;

        public void Setup(ReactiveProperty<TaskState> viewModel)
        {
            _viewModel = viewModel;
        }

        public bool Back()
        {
            _viewModel.Value = TaskState.None;
            return true;
        }

        private void Start()
        {
            _viewModel
                .Select(x => x.Exception.ToShortString())
                .Select(x => x is null || x.Length <= 200 ? x : $"{x[..200]}...")
                .SubscribeToText(messageText)
                .AddTo(this);

            _viewModel.Select(x => !x.IsFaulted).SubscribeToDestroy(gameObject).AddTo(this);

            okButton.OnClickAsObservable().Subscribe(_ => Back());
        }
    }
}
