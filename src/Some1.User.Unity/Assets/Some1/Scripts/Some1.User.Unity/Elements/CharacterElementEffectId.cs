using System;

namespace Some1.User.Unity.Elements
{
    public enum CharacterElementEffectId
    {
        Alive,
        Dead,
        Idle,
        Jump,
        Knock,
        Dash,
        Grab,
        Attack,
        Super,
        Ultra,
        Walk,
        Damage,
    }

    public static class CharacterElementEffectIdHelper
    {
        public static CharacterElementEffectId Parse(string value) => value switch
        {
            nameof(CharacterElementEffectId.Alive) => CharacterElementEffectId.Alive,
            nameof(CharacterElementEffectId.Dead) => CharacterElementEffectId.Dead,
            nameof(CharacterElementEffectId.Idle) => CharacterElementEffectId.Idle,
            nameof(CharacterElementEffectId.Jump) => CharacterElementEffectId.Jump,
            nameof(CharacterElementEffectId.Knock) => CharacterElementEffectId.Knock,
            nameof(CharacterElementEffectId.Dash) => CharacterElementEffectId.Dash,
            nameof(CharacterElementEffectId.Grab) => CharacterElementEffectId.Grab,
            nameof(CharacterElementEffectId.Attack) => CharacterElementEffectId.Attack,
            nameof(CharacterElementEffectId.Super) => CharacterElementEffectId.Super,
            nameof(CharacterElementEffectId.Ultra) => CharacterElementEffectId.Ultra,
            nameof(CharacterElementEffectId.Walk) => CharacterElementEffectId.Walk,
            nameof(CharacterElementEffectId.Damage) => CharacterElementEffectId.Damage,
            _ => throw new InvalidOperationException($"Failed to parse {nameof(CharacterElementEffectId)} '{value}'"),
        };
    }
}
