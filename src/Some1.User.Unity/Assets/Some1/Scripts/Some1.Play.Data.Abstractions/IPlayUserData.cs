using System;
using System.Collections.Generic;

namespace Some1.Play.Data
{
    public interface IPlayUserData
    {
        string Id { get; }
        string PlayId { get; }
        bool IsPlaying { get; }
        bool Manager { get; }
        DateTime? Premium { get; }
        IPackedData? PackedData { get; }
    }

    public interface IPackedData
    {
        IGameArgsGroupData? GameArgses { get; }
        IGameResultGroupData? GameResults { get; }
        ExpData? Exp { get; }
        ICharacterGroupData? Characters { get; }
        ISpecialtyGroupData? Specialties { get; }
        TitleData? Title { get; }
        EmojiData? Emoji { get; }
        WelcomeData? Welcome { get; }
    }

    public interface IGameArgsGroupData
    {
        int SelectedItemId { get; }
        IReadOnlyList<GameArgsData> Items { get; }
    }

    public interface IGameResultGroupData
    {
        IReadOnlyList<IGameResultData> Items { get; }
    }

    public interface IGameResultData
    {
        bool Success { get; }
        GameResultGameData? Game { get; }
        float Cycles { get; }
        DateTime EndTime { get; }
        GameResultCharacterData Character { get; }
        IReadOnlyList<GameResultSpecialtyData> Specialties { get; }
    }

    public interface ICharacterGroupData
    {
        int SelectedItemId { get; }
        DateTime PickTime { get; }
        IReadOnlyList<ICharacterData> Items { get; }
    }

    public interface ICharacterData
    {
        int Id { get; }
        bool IsPicked { get; }
        bool IsRandomSkin { get; }
        int Exp { get; }
        DateTime ExpUtc { get; }
        int SelectedSkinId { get; }
        IReadOnlyList<CharacterSkinData> Skins { get; }
    }

    public interface ISpecialtyGroupData
    {
        IReadOnlyList<SpecialtyData> Items { get; }
    }
}
