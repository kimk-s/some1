using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.User.Unity.Elements;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerFaceView : MonoBehaviour
    {
        public Image iconImage;
        public ElementManager elementManager;

        private PlayerFaceViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetService<PlayViewModel>().Page.Face;
        }

        private void Start()
        {
            _viewModel.CharacterId.Subscribe(x => LoadIconImageAsync(x).Forget()).AddTo(this);

            elementManager.Register(() => { });

            _viewModel.CharacterId.Subscribe(x => elementManager.Path = x?.GetPlayerElementPath()).AddTo(this);

            _viewModel.GameToastState
                .Select(x =>
                {
                    return x.Type switch
                    {
                        PlayerGameToastStateType.None => null,
                        PlayerGameToastStateType.Ready => (PlayerElementEffectId?)PlayerElementEffectId.GameReady,
                        PlayerGameToastStateType.Return => PlayerElementEffectId.GameReturn,
                        PlayerGameToastStateType.ReReady => (PlayerElementEffectId?)PlayerElementEffectId.GameReady,
                        PlayerGameToastStateType.ReturnFaulted => PlayerElementEffectId.GameReturnFaulted,
                        PlayerGameToastStateType.Start => PlayerElementEffectId.GameStart,
                        PlayerGameToastStateType.End => PlayerElementEffectId.GameEnd,
                        _ => throw new InvalidOperationException()
                    };
                })
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Where(x => x is not null)
                .Subscribe(x => PlayElementEffect(x.Value));

            _viewModel.AttackFailNotAnyLoadCount
                .Skip(1)
                .Subscribe(_ => PlayElementEffect(PlayerElementEffectId.AttackFailNotAnyLoad))
                .AddTo(this);

            int attackLoadCount = 0;
            _viewModel.CastItems[CastId.Attack].LoadCount
                .Skip(1)
                .Subscribe(x =>
                {
                    if (x == 0)
                    {
                        PlayElementEffect(PlayerElementEffectId.AttackNotAnyLoad);
                    }
                    else if (x > attackLoadCount)
                    {
                        PlayElementEffect(PlayerElementEffectId.AttackReload);
                    }

                    attackLoadCount = x;
                })
                .AddTo(this);

            _viewModel.CastItems[CastId.Super].AnyLoadCount
                .Skip(1)
                .Where(x => x)
                .Subscribe(_ => PlayElementEffect(PlayerElementEffectId.SuperReload))
                .AddTo(this);

            _viewModel.CastItems[CastId.Ultra].AnyLoadCount
                .Skip(1)
                .Where(x => x)
                .Subscribe(_ => PlayElementEffect(PlayerElementEffectId.UltraReload))
                .AddTo(this);

            int takeStuffsComboScore = 0;
            _viewModel.TakeStuffsComboScore
                .Subscribe(x =>
                {
                    if (x > takeStuffsComboScore)
                    {
                        PlayElementEffect(PlayerElementEffectId.TakeStuff);
                    }

                    takeStuffsComboScore = x;
                })
                .AddTo(this);

            _viewModel.Like
                .Skip(1)
                .Subscribe(_ => PlayElementEffect(PlayerElementEffectId.TakeLike))
                .AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync(CharacterId? x)
        {
            iconImage.sprite = null;

            if (x is null)
            {
                return;
            }

            var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(x.Value.GetIconPath(), destroyCancellationToken);
            if (x != _viewModel.CharacterId.CurrentValue)
            {
                return;
            }

            iconImage.sprite = sprite;
        }

        private PlayerElement? GetElement() => (PlayerElement?)elementManager.Element;

        private void PlayElementEffect(PlayerElementEffectId id)
        {
            var element = GetElement();
            if (element == null || element.effects == null)
            {
                return;
            }

            element.effects.Play(id);
        }
    }
}
