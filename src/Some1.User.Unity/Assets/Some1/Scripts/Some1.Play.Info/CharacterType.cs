namespace Some1.Play.Info
{
    public enum CharacterType : byte
    {
        Static,
        Player,
        NonPlayer,
        Effect,
        Stuff,
    }

    public static class CharacterTypeExtensions
    {
        public static bool IsUnit(this CharacterType x) => x == CharacterType.Player || x == CharacterType.NonPlayer;
    }
}
