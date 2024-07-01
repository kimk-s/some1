namespace Some1.Play.Info
{
    public sealed class CharacterGiveStuffInfo
    {
        public CharacterGiveStuffInfo(Stuff stuff)
        {
            Stuff = stuff;
        }

        public Stuff Stuff { get; }
    }
}
