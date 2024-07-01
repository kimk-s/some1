using Cysharp.Threading.Tasks;
using R3;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerCharacterDetailView : MonoBehaviour, IBackable
    {
        public Button upButton;
        public TMP_Text nameText;

        public Image iconImage;
        public TMP_Text descriptionText;

        public GameObject isUnlockedFalseImage;
        public GameObject isUnlockedTrueImage;
        public TMP_Text isUnlockedText;

        public TMP_Text isPickedText;

        public GameObject isSelectedRadioEnabled;
        public GameObject isSelectedRadioSelected;
        public TMP_Text isSelectedText;

        public TMP_Text starLevelText;
        public TMP_Text starPointText;
        public TMP_Text expTimeAgoText;
        public TMP_Text isRandomSkinText;
        public GameObject skinsContent;
        public TMP_Text roleText;
        public TMP_Text seasonText;
        public TMP_Text healthText;
        public TMP_Text walkSpeedText;
        public GameObject skillsContent;
        public Button selectButton;
        public Button setIsRandomSkinButton;
        public PlayerCharacterSkinView skinViewPrefab;
        public PlayerCharacterSkillView skillViewPrefab;

        private PlayerCharacterDetailViewModel _viewModel;

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        public void Setup(PlayerCharacterDetailViewModel viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        private void Start()
        {
            upButton.OnClickAsObservable().Subscribe(_ => Back());

            _viewModel.Destroy.SubscribeToDestroy(gameObject).AddTo(this);

            _viewModel.Id.GetName().AsRStringObservable().SubscribeToText(nameText).AddTo(this);
            LoadIconImageAsync().Forget();
            _viewModel.Id.GetDescription().AsRStringObservable().SubscribeToText(descriptionText).AddTo(this);

            _viewModel.IsUnlocked.Select(x => !x).SubscribeToActive(isUnlockedFalseImage).AddTo(this);
            _viewModel.IsUnlocked.SubscribeToActive(isUnlockedTrueImage).AddTo(this);
            _viewModel.IsUnlocked
                .Select(x => CommonConverter.GetYesNo(x))
                .AsRStringObservable()
                .SubscribeToText(isUnlockedText)
                .AddTo(this);

            _viewModel.IsPicked
                .Select(x => CommonConverter.GetYesNo(x))
                .AsRStringObservable()
                .SubscribeToText(isPickedText)
                .AddTo(this);

            _viewModel.IsUnlocked.SubscribeToActive(isSelectedRadioEnabled).AddTo(this);
            _viewModel.IsSelected.SubscribeToActive(isSelectedRadioSelected).AddTo(this);
            _viewModel.IsSelected
                .Select(x => CommonConverter.GetYesNo(x))
                .AsRStringObservable()
                .SubscribeToText(isSelectedText)
                .AddTo(this);

            _viewModel.Star.Select(x => StringFormatter.FormatDetailLevel(x)).SubscribeToText(starLevelText).AddTo(this);
            _viewModel.Star.Select(x => StringFormatter.FormatDetailPoint(x)).SubscribeToText(starPointText).AddTo(this);
            _viewModel.ExpTimeAgo.Select(x => StringFormatter.FormatAgo(x)).SubscribeToText(expTimeAgoText).AddTo(this);
            _viewModel.IsRandomSkin
                .Select(x => CommonConverter.GetYesNo(x))
                .AsRStringObservable()
                .SubscribeToText(isRandomSkinText)
                .AddTo(this);

            foreach (var item in _viewModel.Skins.Values)
            {
                skinsContent.Add(skinViewPrefab).Setup(item, _viewModel.ClickSkin);
            }

            _viewModel.Role.Value.GetName().AsRStringObservable().SubscribeToText(roleText).AddTo(this);
            seasonText.text = _viewModel.Season?.GetCode() ?? "-";
            healthText.text = _viewModel.Health.ToString();
            _viewModel.WalkSpeed.GetName().AsRStringObservable().SubscribeToText(walkSpeedText).AddTo(this);

            foreach (var item in _viewModel.Skills)
            {
                skillsContent.Add(skillViewPrefab).Setup(item);
            }

            _viewModel.Select.BindTo(selectButton).AddTo(this);
            _viewModel.SetIsRandomSkin.BindTo(setIsRandomSkinButton, _ => !_viewModel.IsRandomSkin.CurrentValue).AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync()
        {
            iconImage.sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(_viewModel.Id.GetIconPath(), destroyCancellationToken);
        }
    }
}
