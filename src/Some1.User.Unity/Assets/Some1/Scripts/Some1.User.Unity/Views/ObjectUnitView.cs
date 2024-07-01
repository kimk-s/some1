using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitView : MonoBehaviour
    {
        public ElementManager elementManager;
        public ObjectUnitBoosterView booster;
        public ObjectUnitTakeStuffGroupView takeStuffs;
        public ObjectUnitHitGroupView hits;
        public ObjectUnitEmojiView emoji;
        public ObjectUnitLikeGroupView likes;
        public ObjectUnitTitleView title;
        public ObjectUnitEnergyGroupView energies;
        public ObjectUnitCastView cast;
        public ObjectUnitMedalView medal;

        private ObjectUnitViewModel _viewModel;
        private Vector2 _characterSize;

        public Vector2 CharacterSize
        {
            get => _characterSize;

            set
            {
                if (_characterSize == value)
                {
                    return;
                }

                _characterSize = value;
                ApplyCharacterSize();
            }
        }

        public void Setup(ObjectUnitViewModel viewModel)
        {
            booster.Setup(viewModel.Boosters[0], () => GetSubElement(x => x.booster));
            takeStuffs.Setup(viewModel.TakeStuffs, () => GetSubElement(x => x.takeStuffs));
            hits.Setup(viewModel.Hits, () => GetSubElement(x => x.hits));
            emoji.Setup(viewModel.Emoji, () => GetSubElement(x => x.emoji));
            likes.Setup(viewModel.Likes, () => GetSubElement(x => x.likes));
            title.Setup(viewModel.Title, () => GetSubElement(x => x.title));
            energies.Setup(viewModel.Energies, () => GetSubElement(x => x.energies));
            cast.Setup(viewModel.Cast, () => GetSubElement(x => x.cast));
            medal.Setup(viewModel.Medal, () => GetSubElement(x => x.medal));
            _viewModel = viewModel;
        }

        private void Start()
        {
            elementManager.Register(() =>
            {
                ApplyCharacterSize();
                ApplyShiftHeight();
                ApplyPosition();
                ApplyBattle();

                booster.Apply();
                takeStuffs.Apply();
                hits.Apply();
                emoji.Apply();
                likes.Apply();
                title.Apply();
                energies.Apply();
                cast.Apply();
                medal.Apply();
            });

            _viewModel.ShiftHeight.Subscribe(_ => ApplyShiftHeight()).AddTo(this);
            _viewModel.Position.Subscribe(_ => ApplyPosition()).AddTo(this);
            _viewModel.Battle.Subscribe(_ => ApplyBattle()).AddTo(this);

            _viewModel.CharacterType
                .CombineLatest(
                    _viewModel.Relation,
                    (characterType, relation) =>
                    {
                        if (characterType?.IsUnit() != true || relation is null)
                        {
                            return (UnitElementId?)null;
                        }

                        return relation switch
                        {
                            ObjectRelation.Mine => UnitElementId.Mine,
                            ObjectRelation.Ally => (characterType.Value & CharacterType.Player) != 0 ? UnitElementId.AllyPlayer : UnitElementId.AllyNonPlayer,
                            ObjectRelation.Enemy => (characterType.Value & CharacterType.Player) != 0 ? UnitElementId.EnemyPlayer : UnitElementId.EnemyNonPlayer,
                            _ => throw new InvalidOperationException()
                        };
                    })
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(x => elementManager.Path = x?.GetElementPath());
        }

        private TSubElement? GetSubElement<TSubElement>(Func<UnitElement, TSubElement> getSubElement)
            where TSubElement : MonoBehaviour
        {
            var element = GetElement();
            return element == null ? null : getSubElement(element);
        }

        private UnitElement? GetElement() => (UnitElement?)elementManager.Element;

        private void ApplyCharacterSize()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.top.localPosition = new(0, CharacterSize.y + 0.2f, 0);

            float value = CharacterSize.x;
            element.bottom.localScale = new(value, value, 1);
        }

        private void ApplyShiftHeight()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.@base.localPosition = new(0, _viewModel.ShiftHeight.CurrentValue, 0);
        }

        private void ApplyPosition()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            element.transform.localPosition = _viewModel.Position.CurrentValue.ToUnityVector3();
        }

        private void ApplyBattle()
        {
            var element = GetElement();
            if (element == null)
            {
                return;
            }

            bool value = _viewModel.Battle.CurrentValue;

            if (element.title != null)
            {
                element.title.transform.localPosition = new(
                    0,
                    value ? element.titlePositionInBattle : element.titlePositionInNotBattle,
                    0);
            }

            if (element.booster != null)
            {
                element.booster.gameObject.SetActive(value);
            }

            if (element.energies != null)
            {
                element.energies.gameObject.SetActive(value);
            }

            if (element.cast != null)
            {
                element.cast.gameObject.SetActive(value);
            }
        }
    }
}
