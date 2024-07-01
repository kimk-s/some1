using System.Collections.Generic;
using Some1.Play.Info;
using R3;
using System;

namespace Some1.Play.Front
{
    public interface IPlayerCharacterFront
    {
        CharacterId Id { get; }
        CharacterRole? Role { get; }
        SeasonId? Season { get; }
        WalkSpeed WalkSpeed { get; }
        int Health { get; }
        IReadOnlyList<IPlayerCharacterSkillFront> Skills { get; }
        ReadOnlyReactiveProperty<bool> IsUnlocked { get; }
        ReadOnlyReactiveProperty<bool> IsPicked { get; }
        ReadOnlyReactiveProperty<bool> IsSelected { get; }
        ReadOnlyReactiveProperty<bool> IsRandomSkin { get; }
        ReadOnlyReactiveProperty<int> Exp { get; }
        ReadOnlyReactiveProperty<Leveling> Star { get; }
        ReadOnlyReactiveProperty<TimeSpan?> ExpTimeAgo { get; }
        IReadOnlyDictionary<SkinId, IPlayerCharacterSkinFront> Skins { get; }
        ReadOnlyReactiveProperty<IPlayerCharacterSkinFront?> ElectedSkin { get; }
        ReadOnlyReactiveProperty<IPlayerCharacterSkinFront?> SelectedSkin { get; }
    }
}
