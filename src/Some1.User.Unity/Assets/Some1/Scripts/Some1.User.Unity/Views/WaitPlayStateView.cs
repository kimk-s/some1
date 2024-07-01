//using Cysharp.Threading.Tasks;
//using Some1.Resources;
//using Some1.User.Unity.Utilities;
//using Some1.User.ViewModel;
//using TMPro;
//using R3;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Some1.User.Unity.Views
//{
//    public class WaitPlayStateView : MonoBehaviour
//    {
//        public Image countryFlagImage;
//        public TMP_Text primaryText;
//        public TMP_Text secondaryText;

//        private WaitPlayStateViewModel? _viewModel;

//        public WaitPlayStateViewModel? ViewModel
//        {
//            get => _viewModel;

//            set
//            {
//                if (_viewModel == value)
//                {
//                    return;
//                }

//                _viewModel = value;
//                Bind();
//            }
//        }

//        private void Start()
//        {
//            R.Culture.Skip(1).Subscribe(_ => Bind()).AddTo(this);
//        }

//        private void Bind()
//        {
//            LoadCountryFlagImageAsync().Forget();

//            primaryText.text = ViewModel is null
//                ? null
//                : StringFormatter.FormatServerName(ViewModel.Value.Region, ViewModel.Value.City, ViewModel.Value.Number);

//            secondaryText.text = ViewModel is null
//                ? null
//                : StringFormatter.FormatServerStatus(ViewModel.Value.Maintenance, ViewModel.Value.Busy);
//        }

//        private async UniTaskVoid LoadCountryFlagImageAsync()
//        {
//            countryFlagImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(
//                CountryConverter.GetFlagPath(ServerConverter.GetCountryCode(_viewModel.Value.City)),
//                destroyCancellationToken);
//        }
//    }
//}
