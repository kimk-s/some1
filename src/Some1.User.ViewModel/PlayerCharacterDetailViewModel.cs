using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.UI;

namespace Some1.User.ViewModel
{
    public sealed class PlayerCharacterDetailViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerCharacterDetailViewModel(
            IPlayFront front,
            CharacterId characterId,
            MessageListViewModel messageList,
            SharedCanExecute sharedCanExecute)
        {
            var destroy = new ReactiveProperty<bool>().AddTo(_disposables);
            Destroy = destroy;

            var characterFront = front.Player.Characters.All[characterId];
            Id = characterFront.Id;
            IsUnlocked = characterFront.IsUnlocked.Connect().AddTo(_disposables);
            IsPicked = characterFront.IsPicked.Connect().AddTo(_disposables);
            IsSelected = characterFront.IsSelected.Connect().AddTo(_disposables);
            IsRandomSkin = characterFront.IsRandomSkin.Connect().AddTo(_disposables);
            Star = characterFront.Star.Connect().AddTo(_disposables);
            ExpTimeAgo = characterFront.ExpTimeAgo.Connect().AddTo(_disposables);

            Skins = characterFront.Skins.ToDictionary(
                x => x.Key,
                x => new PlayerCharacterSkinViewModel(x.Value, IsRandomSkin).AddTo(_disposables));

            Role = characterFront.Role;
            Season = characterFront.Season;
            Health = characterFront.Health;
            WalkSpeed = characterFront.WalkSpeed;
            Skills = characterFront.Skills;

            Select = new ReactiveCommand<Unit>(sharedCanExecute, true).AddTo(_disposables);
            Select.SubscribeAwait(
                sharedCanExecute,
                async (_, ct) =>
                {
                    var error = await front.SelectCharacterAsync(Id, ct);

                    if (error != PlayFrontError.Success)
                    {
                        messageList.Add.Execute(error);
                        return;
                    }

                    destroy.Value = true;
                },
                AwaitOperation.Drop);

            SetIsRandomSkin = new ReactiveCommand<bool>(sharedCanExecute, true).AddTo(_disposables);
            SetIsRandomSkin.SubscribeAwait(
                sharedCanExecute,
                async (x, ct) =>
                {
                    var error = await front.SetCharacterIsRandomSkinAsync(Id, x, ct);

                    if (error != PlayFrontError.Success)
                    {
                        messageList.Add.Execute(error);
                        return;
                    }
                },
                AwaitOperation.Drop);

            ClickSkin = new ReactiveCommand<SkinId>(sharedCanExecute, true).AddTo(_disposables);
            ClickSkin.SubscribeAwait(
                sharedCanExecute,
                async (x, ct) =>
                {
                    var error = IsRandomSkin.CurrentValue
                        ? await front.SetCharacterSkinIsRandomSelectedAsync(Id, x, !Skins[x].IsRandomSelected.CurrentValue, ct)
                        : await front.SelectCharacterSkinAsync(Id, x, ct);

                    if (error != PlayFrontError.Success)
                    {
                        messageList.Add.Execute(error);
                        return;
                    }
                },
                AwaitOperation.Drop);
        }

        public ReadOnlyReactiveProperty<bool> Destroy { get; }

        public CharacterId Id { get; }

        public ReadOnlyReactiveProperty<bool> IsUnlocked { get; }

        public ReadOnlyReactiveProperty<bool> IsPicked { get; }

        public ReadOnlyReactiveProperty<bool> IsSelected { get; }

        public ReadOnlyReactiveProperty<bool> IsRandomSkin { get; }

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

        public ReadOnlyReactiveProperty<TimeSpan?> ExpTimeAgo { get; }

        public IReadOnlyDictionary<SkinId, PlayerCharacterSkinViewModel> Skins { get; }

        public CharacterRole? Role { get; }

        public SeasonId? Season { get; }

        public int Health { get; }

        public WalkSpeed WalkSpeed { get; }

        public IReadOnlyList<IPlayerCharacterSkillFront> Skills { get; }

        public ReactiveCommand<Unit> Select { get; }

        public ReactiveCommand<bool> SetIsRandomSkin { get; }

        public ReactiveCommand<SkinId> ClickSkin { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
