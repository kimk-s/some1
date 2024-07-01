using System;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.Resources;
using TMPro;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerCharacterSkillPropView : MonoBehaviour
    {
        public TMP_Text nameText;
        public TMP_Text valueText;

        private IPlayerCharacterSkillPropFront _viewModel;

        public void Setup(IPlayerCharacterSkillPropFront viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Prop.Type.GetName().AsRStringObservable().SubscribeToText(nameText).AddTo(this);

            Observable<string> value;
            switch (_viewModel.Prop.Type)
            {
                case SkillPropType.Damage:
                    {
                        var p = (DamageSkillProp)_viewModel.Prop;

                        value = Observable.Return(p.Count switch
                        {
                            < 1 => "",
                            1 => p.Damage.ToString(),
                            _ => $"{p.Count} x {p.Damage}",
                        });
                    }
                    break;
                case SkillPropType.Range:
                    {
                        var p = (RangeSkillProp)_viewModel.Prop;

                        value = p.Value.GetName().AsRStringObservable();
                    }
                    break;
                case SkillPropType.Reload:
                    {
                        var p = (ReloadSkillProp)_viewModel.Prop;

                        value = p.Value.GetName().AsRStringObservable();
                    }
                    break;
                case SkillPropType.Defense:
                    {
                        var p = (DefenseSkillProp)_viewModel.Prop;

                        value = Observable.Return($"{p.Value * 10}%");
                    }
                    break;
                case SkillPropType.Duration:
                    {
                        var p = (DurationSkillProp)_viewModel.Prop;

                        value = R.Culture.Select(_ => StringFormatter.FormatTimeSpanShort(TimeSpan.FromSeconds(p.Value)));
                    }
                    break;
                case SkillPropType.Recovery:
                    {
                        var p = (RecoverySkillProp)_viewModel.Prop;

                        value = Observable.Return(p.Value.ToString());
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
            value.SubscribeToText(valueText).AddTo(this);
        }
    }
}
