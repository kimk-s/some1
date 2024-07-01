using Some1.Play.Front;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using TMPro;
using R3;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerCharacterSkillView : MonoBehaviour
    {
        public TMP_Text iconText;
        public TMP_Text namePrimaryText;
        public TMP_Text nameSecondaryText;
        public TMP_Text descriptionText;
        public GameObject propsContent;
        public PlayerCharacterSkillPropView propViewPrefab;

        private IPlayerCharacterSkillFront _viewModel;

        public void Setup(IPlayerCharacterSkillFront viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            iconText.text = _viewModel.Id.Skill.GetIconString();
            R.Culture.Select(_ => _viewModel.Id.GetName()).AsRStringObservable().SubscribeToText(namePrimaryText).AddTo(this);
            R.Culture.Select(_ => _viewModel.Id.Skill.GetName()).AsRStringObservable().SubscribeToText(nameSecondaryText).AddTo(this);
            R.Culture.Select(_ => _viewModel.Id.GetDescription()).AsRStringObservable().SubscribeToText(descriptionText).AddTo(this);

            foreach (var item in _viewModel.Props.Values)
            {
                propsContent.Add(propViewPrefab).Setup(item);
            }

            LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(gameObject);
        }
    }
}
