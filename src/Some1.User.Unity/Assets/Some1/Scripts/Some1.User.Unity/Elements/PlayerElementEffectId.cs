using System;

namespace Some1.User.Unity.Elements
{
    public enum PlayerElementEffectId
    {
        GameReady,
        GameReturn,
        GameReReady,
        GameReturnFaulted,
        GameStart,
        GameEnd,
        AttackFailNotAnyLoad,
        AttackNotAnyLoad,
        AttackReload,
        SuperReload,
        UltraReload,
        TakeStuff,
        TakeLike,
    }

    public static class PlayerElementEffectIdHelper
    {
        public static PlayerElementEffectId Parse(string value) => value switch
        {
            nameof(PlayerElementEffectId.GameReady) => PlayerElementEffectId.GameReady,
            nameof(PlayerElementEffectId.GameReturn) => PlayerElementEffectId.GameReturn,
            nameof(PlayerElementEffectId.GameReReady) => PlayerElementEffectId.GameReReady,
            nameof(PlayerElementEffectId.GameReturnFaulted) => PlayerElementEffectId.GameReturnFaulted,
            nameof(PlayerElementEffectId.GameStart) => PlayerElementEffectId.GameStart,
            nameof(PlayerElementEffectId.GameEnd) => PlayerElementEffectId.GameEnd,
            nameof(PlayerElementEffectId.AttackFailNotAnyLoad) => PlayerElementEffectId.AttackFailNotAnyLoad,
            nameof(PlayerElementEffectId.AttackNotAnyLoad) => PlayerElementEffectId.AttackNotAnyLoad,
            nameof(PlayerElementEffectId.AttackReload) => PlayerElementEffectId.AttackReload,
            nameof(PlayerElementEffectId.SuperReload) => PlayerElementEffectId.SuperReload,
            nameof(PlayerElementEffectId.UltraReload) => PlayerElementEffectId.UltraReload,
            nameof(PlayerElementEffectId.TakeStuff) => PlayerElementEffectId.TakeStuff,
            nameof(PlayerElementEffectId.TakeLike) => PlayerElementEffectId.TakeLike,
            _ => throw new InvalidOperationException($"Failed to parse {nameof(PlayerElementEffectId)} '{value}'"),
        };
    }
}
