using System;
using System.Drawing;
using System.Numerics;

namespace Some1.Play.Info
{
    public static class PlayConst
    {
        public const int BuffCount = 10;

        public const float UnitSize = 0.9f;

        public const float UnitStrikeScanAimScale = 1.1f;

        public const float UnitShootTriggerPosition = UnitSize * 0.5f;

        public const float UnitShootAimPotition = UnitSize * 0.4f;

        public const int MaxBoosterNumber = 10;

        public const int SpecialtyCount = 10;

        public const int SpecialtyMaxPinCount = 5;

        public const int TakeStuffCount = 10;

        public const int MaxChildCount = 32;

        public const int HitCount = 10;

        public const int MinStatValue = -8;

        public const int MaxStatValue = 20;

        public const float MaxTeleportDeltaLength = 10;

        public const float MaxJumpDeltaLength = 20;

        public const int AutoValue = -1;

        public static readonly Vector2 PlayerEyesightPosition = new(0, 0);

        public const float PlayerEyesightWidth = 31;
        public const float PlayerEyesightHeight = 23;

        public static readonly SizeF PlayerEyesightSize = new(PlayerEyesightWidth, PlayerEyesightHeight );

        public static readonly SizeF LeaderEyesightSize = PlayerEyesightSize * 2;

        public static readonly Vector2 ChildShrinkAreaPosition = PlayerEyesightPosition;

        public static readonly SizeF ChildShrinkAreaSize = LeaderEyesightSize;

        public const float ChildShrinkSeconds = 5;

        public const int CharacterStarLeveling_MaxLevel = 100;

        public const int CharacterStarLeveling_StepFactor = 500;

        public const int SpecialtyStarLeveling_MaxLevel = 30;

        public const int SpecialtyStarLeveling_StepFactor = 3;

        public const float PlayerEmojiDelay = 10;

        public const float PlayerEmojiLikeDelay = 60 * 3;

        public const float ObjectEmojiCycle = 5;

        public const float ObjectEmojiLikeCircleDiameter = 6;

        public const byte LikeLeveling_MaxLevel = 8;

        public const byte LikeLeveling_StepFactor = 10;

        public const int TargetTokenCount = 3;

        public const int PlayerGameResultCount = 10;

        public const int PlayerGameManagerReadySeconds = 5;

        public const int PlayerGameManagerReturnSeconds = 10;

        public const int PlayerGameManagerReReadySeconds = 7;

        public const int PlayerGameManagerReturnFaultedSeconds = 5;

        public const float StandardAspectRatio = 16f / 9;

        public const float StandardFieldOfView = 12.8f;

        public const float HitSeconds = 1.5f;

        public const float HitShortSeconds = 0.3f;

        public const float TakeStuffSeconds = 3;

        public const float TakeStuffComboTime = 3;

        public const float GiveStuffSeconds = 15;

        public const float ChallengeSeconds = 120;

        public const float ChallengeReturnTime = ChallengeSeconds - PlayerGameManagerReturnSeconds;

        public const short MinCastManualToken = 1;

        public const short MaxCastManualToken = 10_000;

        public const short MinCastAutoToken = 10_001;

        public const short MaxCastAutoToken = 20_000;

        public const int RankingCount = 100;

        public const int TagColorPointCount = 3;

        public const float TagColorPointTime = 3;

        public const float PlayerFaultedTime = 1 * 60;

        public const int SectionCountInRegion = 9;

        public const float HitAngleQuiet = -1;

        public static bool IsHitAngleQuiet(float angle) => angle < 0;

        public const int MaxPlayerExp = 1_000;

        public const int PlayerExpChargeValue = 100;

        public const int PlayerExpChargeMinutes = 30;

        public static DateTime StandardDateTime { get; } = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public const float MaxEffectTime = 10;
    }
}
