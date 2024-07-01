using System;

namespace Some1.Play.Info
{
    [Flags]
    public enum CharacterTypeTarget : byte
    {
        None = 0,
        Static = 1,
        Player = 2,
        NonPlayer = 4,
        Effect = 8,
        Stuff = 16,

        Unit = Player | NonPlayer,
        All = Static | Player | NonPlayer | Effect | Stuff,
    }

    public static class CharacterTypeTargetExtensions
    {
        public static bool IsMatch(this CharacterTypeTarget x, CharacterType characterType)
        {
            if (x == CharacterTypeTarget.None)
            {
                return false;
            }

            if (x == CharacterTypeTarget.All)
            {
                return true;
            }

            return (((x & CharacterTypeTarget.Static) != CharacterTypeTarget.None) && characterType == CharacterType.Static)
                || (((x & CharacterTypeTarget.Player) != CharacterTypeTarget.None) && characterType == CharacterType.Player)
                || (((x & CharacterTypeTarget.NonPlayer) != CharacterTypeTarget.None) && characterType == CharacterType.NonPlayer)
                || (((x & CharacterTypeTarget.Effect) != CharacterTypeTarget.None) && characterType == CharacterType.Effect)
                || (((x & CharacterTypeTarget.Stuff) != CharacterTypeTarget.None) && characterType == CharacterType.Stuff);
        }
    }
}
