using Cysharp.Threading.Tasks;
using Some1.Prefs.UI;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class CultureLongView : MonoBehaviour
    {
        public TMP_Text nameText;
        public Button selectButton;

        private CultureLongViewModel _viewModel;

        public void Setup(CultureLongViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            nameText.text = _viewModel.Id.GetLongName();
            _viewModel.Select.BindTo(selectButton).AddTo(this);
        }
    }
}
