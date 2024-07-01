using Cysharp.Threading.Tasks;
using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public sealed class PipeTerminatedView : MonoBehaviour, IBackable
    {
        public Button button;

        private PipeTerminatedViewModel _viewModel;

        public void Setup(PipeTerminatedViewModel viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        public bool Back()
        {
            _viewModel.Quit.Execute(Unit.Default);
            return true;
        }

        private void Start()
        {
            _viewModel.Quit.BindTo(button).AddTo(this);
        }
    }
}
