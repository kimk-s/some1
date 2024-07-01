using Cysharp.Threading.Tasks;
using Some1.Prefs.UI;
using Some1.User.Unity.Utilities;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class CultureShortView : MonoBehaviour
    {
        public new TMP_Text name;
        public Button select;

        private CultureShortViewModel _viewModel;

        public void Setup(CultureShortViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            name.text = _viewModel.Id.GetNativeName();
            _viewModel.Select.BindTo(select).AddTo(this);
            LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(gameObject);
        }
    }
}
