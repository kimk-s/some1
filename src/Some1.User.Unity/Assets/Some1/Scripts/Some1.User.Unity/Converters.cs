using System;
using System.Net.Sockets;
using System.Numerics;
using Grpc.Core;
using Some1.Play.Front;
using Some1.Play.Info;
using Some1.Prefs.Data;
using Some1.Resources;
using Some1.User.Unity.Elements;
using Some1.Wait.Back;
using Some1.Wait.Front;

namespace Some1.User.Unity
{
    public static class CommonConverter
    {
        public static string GetYesNo(bool x) => $"Common_{(x ? "Yes" : "No")}";
    }

    public static class CommonThemeConverter
    {
        public static string GetName(this Theme x) => $"Common_Theme_Name_{x}";
    }

    public static class ServerConverter
    {
        public static string GetRegionName(string? value) => value is null ? "" : $"Server_Region_Name_{value}";

        public static string GetCityName(string? value) => value is null ? "" : $"Server_City_Name_{value}";

        public static string GetCountryCode(string city) => city switch
        {
            "Seoul" => "kr",
            "Daejeon" => "kr",
            "Busan" => "kr",
            "Tokyo" => "jp",
            "Osaka" => "jp",
            "Singapore" => "sg",
            "Mumbai" => "in",
            "HongKong" => "hk",
            "Stockholm" => "se",
            "Sydney" => "au",
            "Frankfurt" => "de",
            "Paris" => "fr",
            "London" => "gb",
            "Ireland" => "ie",
            "Virginia" => "us",
            "Ohio" => "us",
            "California" => "us",
            "Oregon" => "us",
            "Central" => "ca",
            "Calgary" => "ca",
            "Montreal" => "ca",
            "SaoPaulo" => "br",
            _ => "",
        };
    }

    public static class CountryConverter
    {
        public static string GetFlagPath(string code) => $"Textures/CountryFlag_{code}";
    }

    public static class PrefsMusicVolumeConverter
    {
        public static string GetName(this MusicVolume x) => $"Prefs_MusicVolume_Name_{x}";
    }

    public static class PrefsSoundVolumeConverter
    {
        public static string GetName(this SoundVolume x) => $"Prefs_SoundVolume_Name_{x}";
    }

    public static class PrefsUISizeConverter
    {
        public static string GetName(this UISize x) => $"Prefs_UISize_Name_{x}";

        public static Vector2 GetCanvasResolution(this UISize x) => x switch
        {
            UISize.Big => new(1920, 1080),
            UISize.Small => new(2560, 1440),
            _ => throw new InvalidOperationException(),
        };
    }

    public static class PrefsCameraModeConverter
    {
        public static string GetName(this CameraMode x) => $"Prefs_CameraMode_Name_{x}";
    }

    public static class PrefsGraphicQualityConverter
    {
        public static string GetName(this GraphicQuality x) => $"Prefs_GraphicQuality_Name_{x}";
    }

    public static class PrefsFpsConverter
    {
        public static string GetName(this Fps x) => $"Prefs_Fps_Name_{x}";

        public static int GetInteger(this Fps x) => x switch
        {
            Fps.Target60 => 60,
            Fps.Target120 => 120,
            Fps.Target240 => 240,
            Fps.Target30 => 30,
            _ => throw new InvalidOperationException()
        };
    }

    public static class PrefsWalkJoystickHoldConverter
    {
        public static string GetName(this WalkJoystickHold x) => $"Prefs_WalkJoystickHold_Name_{x}";

        public static bool GetBoolean(this WalkJoystickHold x) => x switch
        {
            WalkJoystickHold.False => false,
            WalkJoystickHold.True => true,
            _ => throw new InvalidOperationException(),
        };
    }

    public static class PrefsJitterBufferConverter
    {
        public static string GetName(this JitterBuffer x) => $"Prefs_JitterBuffer_Name_{x}";

        public static float GetValue(this JitterBuffer x) => 1.1f * x switch
        {
            JitterBuffer.Large => 0.15f,
            JitterBuffer.Medium => 0.1f,
            JitterBuffer.Small => 0.05f,
            _ => throw new ArgumentOutOfRangeException(nameof(x))
        };
    }

    public static class PlayConverter
    {
        public static string GetSuccessOrFailure(bool x) => $"Play_{(x ? "Success" : "Failure")}";
    }

    public static class PlayCharacterIdConverter
    {
        public static string GetName(this CharacterId x) => $"Play_Character_Name_{x}";

        public static string GetDescription(this CharacterId x) => $"Play_Character_Description_{x}";

        public static string GetIconPath(this CharacterId x) => new CharacterSkinId(x, SkinId.Skin0).GetIconPath();

        public static string GetPlayerElementPath(this CharacterId x) => $"Prefabs/Elements/PlayerElement_{x}";
    }

    public static class PlayCharacterRoleConverter
    {
        public static string GetName(this CharacterRole x) => $"Play_CharacterRole_{x}";
    }

    public static class PlayCharacterSkinIdConverter
    {
        public static string GetElementPath(this CharacterSkinId x) => $"Prefabs/Elements/CharacterElement_{x.Character}_{x.Skin}";

        public static string GetIconPath(this CharacterSkinId x) => $"Textures/Icon_Character_{x.Character}_{x.Skin}";

        public static string GetName(this CharacterSkinId x) => $"Play_CharacterSkin_Name_{x.Character}_{x.Skin}";
    }

    public static class PlayCharacterSkinEmojiIdConverter
    {
        public static string GetImagePath(this CharacterSkinEmojiId x, byte level)
            => $"Textures/Emoji_{x.Character?.ToString() ?? "Common"}_{x.Skin?.ToString() ?? "Common"}_{x.Emoji}{(x.Emoji != EmojiId.Thumbsup ? "" : $"_{Math.Clamp(level, (byte)0, PlayConst.LikeLeveling_MaxLevel)}")}";
    }

    public static class PlayBoosterIdConverter
    {
        public static string GetImagePath(this BoosterId x) => $"Textures/Booster_{x}";
    }

    public static class PlaySpecialtyIdConverter
    {
        public static string GetName(this SpecialtyId x) => $"Play_Specialty_Name_{x}";

        public static string GetDescription(this SpecialtyId x) => $"Play_Specialty_Description_{x}";

        public static string GetIconPath(this SpecialtyId x) => $"Textures/Icon_Specialty_{x}";
    }

    public static class StoreProductIdConverter
    {
        public static string? GetOffer(string productId) => productId switch
        {
            "premium90d" => "-20%",
            "premium180d" => "-25%",
            _ => null
        };

        public static string GetImagePath(string productId) => $"Textures/Product_{productId}";
    }

    public static class PlaySeasonIdConverter
    {
        public static string GetCode(this SeasonId x) => ((int)x + 1).ToString();
    }

    public static class PlayUnaryResultConverter
    {
        public static string GetFormat(this UnaryResult x) => x.Unary is null ? "" : $"Play_UnaryResult_Format_{x.Unary.Value.Type}";

        public static string GetParam1(this UnaryResult x) => x.Unary is null ? "" : x.Unary.Value.Type switch
        {
            _ => "",
        };
    }

    public static class PlayFrontErrorConverter
    {
        public static string GetMessage(this PlayFrontError x) => $"Play_FrontError_Message_{x}";
    }

    public static class PlayMedalConverter
    {
        public static string GetName(this Medal x) => $"Play_Medal_Name_{x}";

        public static string GetImagePath(this Medal x) => $"Textures/Medal_{x}";

        public static string GetFXMaterialPath(this Medal x) => $"Materials/FX_MedalAB_{x}";
    }

    public static class PlayGameModeConverter
    {
        public static string GetName(this GameMode x) => $"Play_GameMode_Name_{x}";

        public static string GetDescription(this GameMode x) => $"Play_GameMode_Description_{x}";

        public static string GetIconString(this GameMode x) => x switch
        {
            GameMode.Challenge => "ch",
            GameMode.Adventure => "ad",
            _ => throw new InvalidOperationException(),
        };
    }

    public static class PlayRankingConverter
    {
        public static string GetNumberFormat(byte x) => x switch
        {
            1 => "Play_Ranking_Number_Format_First",
            2 => "Play_Ranking_Number_Format_Second",
            3 => "Play_Ranking_Number_Format_Third",
            _ => "Play_Ranking_Number_Format",
        };
    }

    public static class PlayRegionSectionIdConverter
    {
        public static string GetCode(this RegionSectionId x) => $"{x.Region.GetCode()}-{PlaySectionIdConverter.GetCode(x.Section)}";

        public static ColorId GetColorId(this RegionSectionId x) =>
            ((int)x.Region * PlayConst.SectionCountInRegion + x.Section) % 2 == 0
                ? ColorId.PaletteA1
                : ColorId.PaletteA4;
    }

    public static class PlayRegionIdConverter
    {
        public static string GetCode(this RegionId x) => x == RegionId.RegionAlpha ? "A" : ((int)x + 1).ToString();
    }

    public static class PlaySectionIdConverter
    {
        public static string GetCode(int x) => (x + 1).ToString();
    }

    public static class PlaySectionTypeConverter
    {
        public static string GetName(this SectionType x) => $"Play_SectionType_Name_{x}";
    }

    public static class PlayFloorTypeConverter
    {
        public static string GetElementPath(this FloorType x) => $"Prefabs/Elements/FloorElement_{x.RegionId}_{(x.SectionKind == SectionKind.None ? "" : x.SectionKind.ToString() + "_")}{(x.Role == FloorRole.Plaza ? FloorRole.Road : x.Role)}_V{x.Variation + 1}";
    }

    public static class PlayBuffSkinIdConverter
    {
        public static string GetElementPath(this BuffSkinId x) => $"Prefabs/Elements/BuffElement_{x.Buff}_{x.Skin}";
    }

    public static class PlayUnitElementIdConverter
    {
        public static string GetElementPath(this UnitElementId x) => $"Prefabs/Elements/UnitElement_{x}";
    }

    public static class PlayStuffConverter
    {
        public static string GetIconPath(this Stuff x) => x.Type switch
        {
            StuffType.Booster => ((BoosterStuff)x).Id.GetImagePath(),
            StuffType.Specialty => ((SpecialtyStuff)x).Id.GetIconPath(),
            _ => throw new InvalidOperationException()
        };
    }

    public static class PlayLikeConverter
    {
        public static string GetImagePath(byte level) => new CharacterSkinEmojiId(null, null, EmojiId.Thumbsup).GetImagePath(level);
    }

    public static class CharacterSkillIdConverter
    {
        public static string GetName(this CharacterSkillId x) => $"Play_Character_Skill_Name_{x.Character}_{x.Skill}";

        public static string GetDescription(this CharacterSkillId x) => $"Play_Character_Skill_Description_{x.Character}_{x.Skill}";
    }

    public static class SkillIdConverter
    {
        public static string GetIconString(this SkillId x) => x switch
        {
            SkillId.Skill1 => "○",
            SkillId.Skill2 => "Ｘ",
            SkillId.Skill3 => "△",
            SkillId.Skill4 => "□",
            _ => "-"
        };

        public static string GetName(this SkillId x) => $"Play_Skill_Name_{x}";
    }

    public static class SkillPropTypeConverter
    {
        public static string GetName(this SkillPropType x) => $"Play_SkillPropType_{x}";
    }

    public static class WalkSpeedConverter
    {
        public static string GetName(this WalkSpeed x) => $"Play_WalkSpeed_{x}";
    }

    public static class SkillRangeConverter
    {
        public static string GetName(this SkillRange x) => $"Play_SkillRange_{x}";
    }

    public static class SkillReloadConverter
    {
        public static string GetName(this SkillReload x) => $"Play_SkillReload_{x}";
    }

    public static class TraitConverter
    {
        public static string GetIconString(this Trait x) => x switch
        {
            Trait.One => SkillId.Skill3.GetIconString(),
            Trait.Two => SkillId.Skill4.GetIconString(),
            _ => ""
        };
    }

    public static class WaitErrorConverter
    {
        public static string GetMessage(Exception? exception)
        {
            return exception switch
            {
                RpcException _ => R.GetString("Wait_Error_Rpc"),
                WaitFrontException ex => R.GetString($"Wait_Error_Front_{ex.Error}"),
                null => "",
                _ => exception.Message
            };
        }
    }

    public static class PlayErrorConverter
    {
        public static string GetMessage(Exception? exception)
        {
            return exception switch
            {
                SocketException _ => R.GetString("Play_Error_Socket"),
                null => "",
                _ => exception.Message
            };
        }
    }

    public static class WaitBuyProductResultConverter
    {
        public static string GetMessage(this WaitBuyProductResult x) => $"Wait_BuyProductResult_Message_{x}";
    }

    public static class WaitWaitPremiumLogReasonConverter
    {
        public static string GetString(this WaitPremiumLogReason x) => $"Wait_WaitPremiumLogReason_{x}";
    }

    public static class TalkIdConverter
    {
        public static string GetName(this TalkId x) => $"Talk_{x}";
    }

    public static class TalkMessageConverter
    {
        public static string GetString(TalkId talkId, string messageId) => $"TalkMessage_{talkId}_{messageId}";
    }

    public static class PlayerIdConverter
    {
        public static string GetMark(this PlayerId x)
        {
            var chars = x.ToString().AsSpan();
            char c1 = chars.Length < 1 ? ' ' : chars[0];
            char c2 = chars.Length < 2 ? ' ' : chars[1];
            char c3 = chars.Length < 3 ? ' ' : chars[2];
            char c4 = chars.Length < 4 ? ' ' : chars[3];
            return $"{c1}{c2}{Environment.NewLine}{c3}{c4}";
        }

        public static UnityEngine.Color GetColor(this PlayerId x)
        {
            var chars = x.ToString().AsSpan();
            float r = chars.Length < 2 ? 0 : GetValue(chars[1]);
            float g = chars.Length < 3 ? 0 : GetValue(chars[2]);
            float b = chars.Length < 4 ? 0 : GetValue(chars[3]);

            return new UnityEngine.Color(r, g, b);

            static float GetValue(char c) => c switch
            {
                '0' => 0 / 15f,
                '1' => 1 / 15f,
                '2' => 2 / 15f,
                '3' => 3 / 15f,
                '4' => 4 / 15f,
                '5' => 5 / 15f,
                '6' => 6 / 15f,
                '7' => 7 / 15f,
                '8' => 8 / 15f,
                '9' => 9 / 15f,
                'A' => 10 / 15f,
                'B' => 11 / 15f,
                'C' => 12 / 15f,
                'D' => 13 / 15f,
                'E' => 14 / 15f,
                'F' => 15 / 15f,
                _ => 0 / 15f
            };
        }
    }
}
