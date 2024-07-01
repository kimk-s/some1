namespace Some1.Play.Info
{
    public sealed class CharacterStatInfo
    {
        public CharacterStatInfo(CharacterStatId id, int value)
        {
            Id = id;
            Value = value;
        }

        public CharacterStatId Id { get; }

        public int Value { get; }
    }
}
