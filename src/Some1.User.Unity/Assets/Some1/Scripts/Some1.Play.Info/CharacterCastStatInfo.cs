namespace Some1.Play.Info
{
    public sealed class CharacterCastStatInfo
    {
        public CharacterCastStatInfo(CharacterCastId id, Trait condition, StatId statId, int statValue)
        {
            Id = id;
            Condition = condition;
            StatId = statId;
            StatValue = statValue;
        }

        public CharacterCastId Id { get; }

        public Trait Condition { get; }

        public StatId StatId { get; }

        public int StatValue { get; }
    }
}
