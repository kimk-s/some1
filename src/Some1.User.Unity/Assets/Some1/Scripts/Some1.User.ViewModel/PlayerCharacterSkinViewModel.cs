using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayerCharacterSkinViewModel : IDisposable
    {
        public PlayerCharacterSkinViewModel(IPlayerCharacterSkinFront front, ReadOnlyReactiveProperty<bool> isRandomSkin)
        {
            Id = front.Id;
            IsUnlocked = front.IsUnlocked;
            IsElected = front.IsElected;
            IsSelected = front.IsSelected;
            IsRandomSelected = front.IsRandomSelected;
            IsRandomSkin = isRandomSkin;
        }

        public CharacterSkinId Id { get; }

        public ReadOnlyReactiveProperty<bool> IsUnlocked { get; }

        public ReadOnlyReactiveProperty<bool> IsElected { get; }

        public ReadOnlyReactiveProperty<bool> IsSelected { get; }

        public ReadOnlyReactiveProperty<bool> IsRandomSelected { get; }

        public ReadOnlyReactiveProperty<bool> IsRandomSkin { get; }

        public void Dispose()
        {
        }
    }
}
