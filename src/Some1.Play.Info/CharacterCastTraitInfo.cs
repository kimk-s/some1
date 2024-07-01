namespace Some1.Play.Info
{
    public sealed class CharacterCastTraitInfo
    {
        public CharacterCastTraitInfo(Trait condition)
        {
            Condition = condition;
        }

        public Trait Condition { get; }
    }
}
