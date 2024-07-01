using System.Collections.Generic;

namespace Some1.Play.Info.Alpha
{
    public static class AlphaTalkInfosFactory
    {
        public static IEnumerable<TalkInfo> Create() => new TalkInfo[]
        {
            new(
                TalkId.Welcome,
                new TalkMessageInfo[]
                {
                    new("1", CharacterId.Player1),
                    new("2", CharacterId.Player2),
                    new("3", CharacterId.Player3),
                    new("4", CharacterId.Player4),
                    new("5", CharacterId.Player5),
                    new("6", CharacterId.Player6),
                    new("7", CharacterId.Player4),
                    new("8", CharacterId.Player1),
                }),
            new(
                TalkId.Server,
                new TalkMessageInfo[]
                {
                    new("1", CharacterId.Player1),
                    new("2", CharacterId.Player4),
                    new("3", CharacterId.Player1),
                    new("4", CharacterId.Player4),
                }),
            new(
                TalkId.PlayerId,
                new TalkMessageInfo[]
                {
                    new("1", CharacterId.Player1),
                    new("2", CharacterId.Player4),
                }),
            new(
                TalkId.DailyCharacter,
                new TalkMessageInfo[]
                {
                    new("1", CharacterId.Player1),
                    new("2", CharacterId.Player4),
                }),
            new(
                TalkId.SurvivalReturnReward,
                new TalkMessageInfo[]
                {
                    new("1", CharacterId.Player1),
                    new("2", CharacterId.Player4),
                }),
            new(
                TalkId.ScoreRanking,
                new TalkMessageInfo[]
                {
                    new("1", CharacterId.Player1),
                    new("2", CharacterId.Player4),
                }),
            new(
                TalkId.GameResult,
                new TalkMessageInfo[]
                {
                    new("1", CharacterId.Player1),
                    new("2", CharacterId.Player4),
                }),
            new(
                TalkId.ExitOnField,
                new TalkMessageInfo[]
                {
                    new("1", CharacterId.Player1),
                    new("2", CharacterId.Player4),
                }),
        };
    }
}
