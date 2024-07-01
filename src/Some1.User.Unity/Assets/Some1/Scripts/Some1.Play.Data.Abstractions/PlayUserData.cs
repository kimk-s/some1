using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Some1.Play.Data
{
    public class PlayUserData : IPlayUserData
    {
        public string Id { get; set; } = "";
        public string PlayId { get; set; } = "";
        public bool IsPlaying { get; set; }
        public bool Manager { get; set; }
        public DateTime? Premium { get; set; }
        public PackedData? PackedData { get; set; }

        IPackedData? IPlayUserData.PackedData => PackedData;
    }

    public class PackedData : IPackedData
    {
        public GameArgsGroupData? GameArgses { get; set; }
        public GameResultGroupData? GameResults { get; set; }
        public ExpData? Exp { get; set; }
        public CharacterGroupData? Characters { get; set; }
        public SpecialtyGroupData? Specialties { get; set; }
        public TitleData? Title { get; set; }
        public EmojiData? Emoji { get; set; }
        public WelcomeData? Welcome { get; set; }

        IGameArgsGroupData? IPackedData.GameArgses => GameArgses;

        IGameResultGroupData? IPackedData.GameResults => GameResults;

        ICharacterGroupData? IPackedData.Characters => Characters;

        ISpecialtyGroupData? IPackedData.Specialties => Specialties;
    }

    public class GameArgsGroupData : IGameArgsGroupData
    {
        public int SelectedItemId { get; set; }
        public GameArgsData[] Items { get; set; } = null!;

        IReadOnlyList<GameArgsData> IGameArgsGroupData.Items => Items;
    }

    public readonly struct GameArgsData
    {
        [JsonConstructor]
        public GameArgsData(int id, int score)
        {
            Id = id;
            Score = score;
        }

        public int Id { get; }
        public int Score { get; }
    }

    public class GameResultGroupData : IGameResultGroupData
    {
        public GameResultData[] Items { get; set; } = null!;

        IReadOnlyList<IGameResultData> IGameResultGroupData.Items => Items;
    }

    public class GameResultData : IGameResultData
    {
        public bool Success { get; set; }
        public GameResultGameData? Game { get; set; }
        public float Cycles { get; set; }
        public DateTime EndTime { get; set; }
        public GameResultCharacterData Character { get; set; }
        public List<GameResultSpecialtyData> Specialties { get; set; } = null!;

        IReadOnlyList<GameResultSpecialtyData> IGameResultData.Specialties => Specialties;
    }

    public readonly struct GameResultGameData
    {
        [JsonConstructor]
        public GameResultGameData(int mode, int score)
        {
            Mode = mode;
            Score = score;
        }

        public int Mode { get; }
        public int Score { get; }
    }

    public readonly struct GameResultCharacterData
    {
        [JsonConstructor]
        public GameResultCharacterData(int id, int exp)
        {
            Id = id;
            Exp = exp;
        }

        public int Id { get; }
        public int Exp { get; }
    }

    public readonly struct GameResultSpecialtyData
    {
        [JsonConstructor]
        public GameResultSpecialtyData(int id, int number)
        {
            Id = id;
            Number = number;
        }

        public int Id { get; }
        public int Number { get; }
    }

    public readonly struct ExpData
    {
        [JsonConstructor]
        public ExpData(int value, DateTime chargeTime)
        {
            Value = value;
            ChargeTime = chargeTime;
        }

        public int Value { get; }
        public DateTime ChargeTime { get; }
    }

    public class CharacterGroupData : ICharacterGroupData
    {
        public int SelectedItemId { get; set; }
        public DateTime PickTime { get; set; }
        public CharacterData[] Items { get; set; } = null!;

        IReadOnlyList<ICharacterData> ICharacterGroupData.Items => Items;
    }

    public class CharacterData : ICharacterData
    {
        public int Id { get; set; }
        public bool IsPicked { get; set; }
        public bool IsRandomSkin { get; set; }
        public int Exp { get; set; }
        public DateTime ExpUtc { get; set; }
        public int SelectedSkinId { get; set; }
        public CharacterSkinData[] Skins { get; set; } = null!;

        IReadOnlyList<CharacterSkinData> ICharacterData.Skins => Skins;
    }

    public readonly struct CharacterSkinData
    {
        [JsonConstructor]
        public CharacterSkinData(int id, bool isRandomSelected)
        {
            Id = id;
            IsRandomSelected = isRandomSelected;
        }

        public int Id { get; }
        public bool IsRandomSelected { get; }
    }

    public class SpecialtyGroupData : ISpecialtyGroupData
    {
        public SpecialtyData[] Items { get; set; } = null!;

        IReadOnlyList<SpecialtyData> ISpecialtyGroupData.Items => Items;
    }

    public readonly struct SpecialtyData
    {
        [JsonConstructor]
        public SpecialtyData(int id, int number, DateTime numberUtc)
        {
            Id = id;
            Number = number;
            NumberUtc = numberUtc;
        }

        public int Id { get; }
        public int Number { get; }
        public DateTime NumberUtc { get; }
    }

    public readonly struct TitleData
    {
        [JsonConstructor]
        public TitleData(int likePoint)
        {
            LikePoint = likePoint;
        }

        public int LikePoint { get; }
    }

    public readonly struct EmojiData
    {
        [JsonConstructor]
        public EmojiData(float likeDelay)
        {
            LikeDelay = likeDelay;
        }

        public float LikeDelay { get; }
    }

    public readonly struct WelcomeData
    {
        [JsonConstructor]
        public WelcomeData(bool value)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
