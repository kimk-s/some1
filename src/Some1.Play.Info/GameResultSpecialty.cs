namespace Some1.Play.Info
{
    public readonly struct GameResultSpecialty
    {
        public GameResultSpecialty(SpecialtyId id, int number)
        {
            Id = id;
            Number = number;
        }

        public SpecialtyId Id { get; }

        public int Number { get; }
    }
}
