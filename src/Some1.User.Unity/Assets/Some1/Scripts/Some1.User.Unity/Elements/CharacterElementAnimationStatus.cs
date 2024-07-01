using System;

namespace Some1.User.Unity.Elements
{
    public enum CharacterElementAnimationStatus
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
    }

    public static class CharacterElementAnimationStatusHelper
    {
        public static string GetString(this CharacterElementAnimationStatus x) => x switch
        {
            CharacterElementAnimationStatus.Alive => nameof(CharacterElementAnimationStatus.Alive),
            CharacterElementAnimationStatus.Dead => nameof(CharacterElementAnimationStatus.Dead),
            CharacterElementAnimationStatus.Idle => nameof(CharacterElementAnimationStatus.Idle),
            CharacterElementAnimationStatus.Jump => nameof(CharacterElementAnimationStatus.Jump),
            CharacterElementAnimationStatus.Knock => nameof(CharacterElementAnimationStatus.Knock),
            CharacterElementAnimationStatus.Dash => nameof(CharacterElementAnimationStatus.Dash),
            CharacterElementAnimationStatus.Grab => nameof(CharacterElementAnimationStatus.Grab),
            CharacterElementAnimationStatus.Attack => nameof(CharacterElementAnimationStatus.Attack),
            CharacterElementAnimationStatus.Super => nameof(CharacterElementAnimationStatus.Super),
            CharacterElementAnimationStatus.Ultra => nameof(CharacterElementAnimationStatus.Ultra),
            CharacterElementAnimationStatus.Walk => nameof(CharacterElementAnimationStatus.Walk),
            _ => throw new InvalidOperationException()
        };

        public static CharacterElementEffectId ToEffectId(this CharacterElementAnimationStatus x) => x switch
        {
            CharacterElementAnimationStatus.Alive => CharacterElementEffectId.Alive,
            CharacterElementAnimationStatus.Dead => CharacterElementEffectId.Dead,
            CharacterElementAnimationStatus.Idle => CharacterElementEffectId.Idle,
            CharacterElementAnimationStatus.Jump => CharacterElementEffectId.Jump,
            CharacterElementAnimationStatus.Knock => CharacterElementEffectId.Knock,
            CharacterElementAnimationStatus.Dash => CharacterElementEffectId.Dash,
            CharacterElementAnimationStatus.Grab => CharacterElementEffectId.Grab,
            CharacterElementAnimationStatus.Attack => CharacterElementEffectId.Attack,
            CharacterElementAnimationStatus.Super => CharacterElementEffectId.Super,
            CharacterElementAnimationStatus.Ultra => CharacterElementEffectId.Ultra,
            CharacterElementAnimationStatus.Walk => CharacterElementEffectId.Walk,
            _ => throw new InvalidOperationException()
        };
    }
}
