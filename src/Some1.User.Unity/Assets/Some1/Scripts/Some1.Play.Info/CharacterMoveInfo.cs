namespace Some1.Play.Info
{
    public sealed class CharacterMoveInfo
    {
        public CharacterMoveInfo(BumpLevel bumpLevel)
        {
            BumpLevel = bumpLevel;
        }

        public BumpLevel BumpLevel { get; }
    }
}
