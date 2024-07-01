using Cysharp.Threading.Tasks;
using R3;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameSpecialtyView : MonoBehaviour
    {
        public Image iconImage;
        public TMP_Text primaryText;
        public TMP_Text secondaryText;
        public GameObject isPinned;
        public GameObject isNotPinned;
        public Button pinButton;

        private PlayerGameSpecialtyViewModel _viewModel;

        public void Setup(PlayerGameSpecialtyViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Specialty.Select(x => x is not null).SubscribeToActive(gameObject).AddTo(this);
        }

        private void Start()
        {
            _viewModel.Specialty
                .Select(x => x?.Id)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(path => LoadIconImageAsync(path).Forget());

            _viewModel.Specialty
                .Select(x => x is null)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(_ => LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(gameObject));

            _viewModel.Specialty
                .Select(x => x is null ? "" : x.Value.Id.GetName())
                .AsRStringObservable()
                .SubscribeToText(primaryText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Specialty,
                    (_, specialty) => StringFormatter.FormatCount(specialty?.Number ?? 0))
                .SubscribeToText(secondaryText)
                .AddTo(this);

            _viewModel.Specialty
                .Select(x => x?.IsPinned == true)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .SubscribeToActive(isPinned);

            _viewModel.Specialty
                .Select(x => x?.IsPinned == false)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .SubscribeToActive(isNotPinned);

            _viewModel.Pin.BindTo(pinButton).AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync(SpecialtyId? id)
        {
            if (id is null)
            {
                iconImage.sprite = null;
            }
            else
            {
                iconImage.sprite = null;

                var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(id.Value.GetIconPath(), destroyCancellationToken);
                if (id != _viewModel.Specialty.CurrentValue?.Id)
                {
                    return;
                }

                iconImage.sprite = sprite;
            }
        }
    }
}
