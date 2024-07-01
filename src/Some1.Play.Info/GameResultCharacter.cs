namespace Some1.Play.Info
{
    public readonly struct GameResultCharacter
    {
        public GameResultCharacter(CharacterId id, int exp)
        {
            Id = id;
            Exp = exp;
        }

        public CharacterId Id { get; }

        public int Exp { get; }
    }
}
