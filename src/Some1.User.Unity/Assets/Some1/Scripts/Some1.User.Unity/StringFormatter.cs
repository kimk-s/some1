using System;
using Some1.Play.Info;
using Some1.Resources;
using Some1.Wait.Front;

namespace Some1.User.Unity.Views
{
    public static class StringFormatter
    {
        public static string FormatBriefLevelWithStar(Leveling x) => $"★ {FormatBriefLevel(x)}";

        public static string FormatBriefPointWithXP(Leveling x) => $"XP {FormatBriefPoint(x)}";

        public static string FormatBriefPointWithEA(Leveling x) => $"EA {FormatBriefPoint(x)}";

        public static string FormatBriefLevel(Leveling x) => $"{x.Level:N0}";

        public static string FormatDetailLevel(Leveling x) => $"{x.Level:N0}/{x.MaxLevel:N0}";

        public static string FormatBriefPoint(Leveling x) => $"{x.Point:N0}";

        public static string FormatDetailPointWithStar(Leveling x) => $"★ {FormatDetailPoint(x)}";

        public static string FormatDetailPoint(Leveling x) => $"{x.Point:N0}/{x.StepMaxPoint:N0} ({x.StepPointRate * 100:F1}%)";

        public static string FormatGiveCash(int x) => $"{x} won";

        public static string FormatTakePremium(int x) => $"Premium {x} days";

        public static string FormatPeriod(DateTime from, DateTime to) => $"{from:yyyy-MM-dd} ~ {to:yyyy-MM-dd}";

        public static string FormatServerName(WaitPlayServerFrontId id) => FormatServerName(id, R.Culture.CurrentValue);

        public static string FormatServerName(WaitPlayServerFrontId id, Culture culture) => FormatServerName(id.Region, id.City, culture);

        public static string FormatServerName(string region, string city) => FormatServerName(region, city, R.Culture.CurrentValue);

        public static string FormatServerName(string region, string city, Culture culture)
            => $"{R.GetString(ServerConverter.GetRegionName(region), culture)} / {R.GetString(ServerConverter.GetCityName(city), culture)}";

        public static string FormatServerStatus(bool openingSoon, bool maintenance, bool isFull)
        {
            if (openingSoon)
            {
                return $"<color=#0000ff>{R.GetString("Wait_OpeningSoon")}</color>";
            }

            if (maintenance)
            {
                return $"<color=#ff00ff>{R.GetString("Wait_Maintenance")}</color>";
            }

            if (isFull)
            {
                return $"<color=#ff0000>{R.GetString("Wait_Full")}</color>";
            }

            return "";
        }

        public static string FormatChannelName(int number)
            => $"{R.GetString("Server_Channel")} {number}";

        public static string FormatChannelStatus(bool openingSoon, bool maintenance, float busy)
        {
            if (openingSoon)
            {
                return $"<color=#0000ff>{R.GetString("Wait_OpeningSoon")}</color>";
            }

            if (maintenance)
            {
                return $"<color=#ff00ff>{R.GetString("Wait_Maintenance")}</color>";
            }

            if (busy >= 1)
            {
                return $"<color=#ff0000>{R.GetString("Wait_Full")}</color>";
            }

            if (busy >= 0.7f)
            {
                return $"{R.GetString("Wait_Busy")}";
            }

            return $"{R.GetString("Wait_NotBusy")}";
        }

        public static string FormatRankingTimeLeftUntilUpdate(int x)
        {
            if (x <= 0 || x > 60)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            return x > 55
                ? R.GetString("Play_Ranking_Updated")
                : $"{R.GetString("Play_Ranking_TimeLeftUntilUpdate")}: {FormatTimeSpanShort(TimeSpan.FromSeconds(x))}";
        }

        public static string FormatAgo(TimeSpan? x) => x is null ? "-" : FormatAgo(x.Value);

        public static string FormatAgo(TimeSpan x)
        {
            string key;
            double value;

            if (x.TotalDays >= 1)
            {
                key = "Days";
                value = x.TotalDays;
            }
            else if (x.TotalHours >= 1)
            {
                key = "Hours";
                value = x.TotalHours;
            }
            else if (x.TotalMinutes >= 1)
            {
                key = "Minutes";
                value = x.TotalMinutes;
            }
            else
            {
                //key = "Seconds";
                key = "Now";
                value = x.TotalSeconds;
            }

            return string.Format(R.GetString($"Play_Ago_Format_{key}"), (int)Math.Floor(value));
        }

        public static string FormatColonTime(TimeSpan x)
        {
            if (x.TotalDays >= 1)
            {
                return $"{x.Days:N0}d {x.Hours:00}:{x.Minutes:00}:{x.Seconds:00}";
            }
            else if (x.TotalHours >= 1)
            {
                return $"{x.Hours:00}:{x.Minutes:00}:{x.Seconds:00}";
            }
            else if (x.TotalMinutes >= 1)
            {
                return $"{x.Minutes:00}:{x.Seconds:00}";
            }
            else
            {
                return $"{0:00}:{x.Seconds:00}";
            }
        }

        public static string FormatScore(int? x) => x is null ? "-" : string.Format(R.GetString("Play_Common_Score_Format"), x);

        public static string FormatSuccessOrFailure(bool x) => $"<color=#{(x ? "2196f3" : "f44336")}>{R.GetString(PlayConverter.GetSuccessOrFailure(x))}</color>";

        public static string FormatCount(int? x) => x is null ? "-" : string.Format(R.GetString("Play_Common_Count_Format"), x);

        public static string FormatGameResultPrimaryText(bool success, GameMode mode, int score)
            => $"{R.GetString(PlayGameModeConverter.GetName(mode))} - {FormatScore(score)} - {R.GetString(PlayConverter.GetSuccessOrFailure(success))}";

        public static string FormatSeasonCodeName(SeasonId x) => $"{R.GetString("Play_Season")} {x.GetCode()}";

        //public static string FormatRegionSectionCodeName(RegionSectionId x) => $"{FormatRegionCodeName(x.Region)} - {FormatSectionCodeName(x.Section)}";

        public static string FormatRegionCodeName(RegionId x) => $"{R.GetString("Play_Region")} {x.GetCode()}";

        public static string FormatSectionCodeName(int x) => $"{R.GetString("Play_Section")} {PlaySectionIdConverter.GetCode(x)}";

        public static string FormatPlayerExp(int value, TimeSpan chargeTimeRemained)
            => $"{R.GetString("Play_GameReady_ObtainableXP")}: {value}{(chargeTimeRemained == TimeSpan.Zero ? "" : $" (+{PlayConst.PlayerExpChargeValue}: {FormatTimeSpanShort(chargeTimeRemained)})")}";

        public static string FormatPickTimeRemained(TimeSpan x)
            => x.TotalSeconds >= 0
                ? $"{R.GetString("Play_CharacterGroup_PickTimeRemained")}: {FormatTimeSpanShort(x)}"
                : R.GetString("Play_CharacterGroup_PickWhenReturn");

        public static string FormatReturnAfter(TimeSpan x)
            => x.TotalSeconds >= 0
                ? string.Format(R.GetString("Play_GameDetail_ReturnAfter"), FormatTimeSpanShort(x))
                : R.GetString("Play_GameDetail_ReturnAfter");

        public static string FormatTimeSpanShort(TimeSpan x)
        {
            if (x.TotalDays >= 1)
            {
                return string.Format(R.GetString("Play_TimeSpan_Short_Format_Days"), x.Days, x.Hours, x.Minutes);
            }
            else if (x.TotalHours >= 1)
            {
                return string.Format(R.GetString("Play_TimeSpan_Short_Format_Hours"), x.Hours, x.Minutes);
            }
            else if (x.TotalMinutes >= 1)
            {
                return string.Format(R.GetString("Play_TimeSpan_Short_Format_Minutes"), x.Minutes, x.Seconds);
            }
            else
            {
                return string.Format(R.GetString("Play_TimeSpan_Short_Format_Seconds"), x.Seconds);
            }
        }

        public static string FormatPremiumLogChangedDays(int x) => x >= 0 ? $"+{x}" : $"-{x}";

        public static string FormatGameSpecialtyCount(int x) => $"{x}/{PlayConst.SpecialtyCount}";

        public static string FormatBoosterNumber(int x) => x >= PlayConst.MaxBoosterNumber ? "M" : x.ToString();
    }
}
