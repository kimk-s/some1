namespace Some1.Play.Info
{
    public sealed class BuffHitInfo
    {
        public BuffHitInfo(Trait condition, HitId hitId, int baseValue, int traitValue)
        {
            Condition = condition;
            HitId = hitId;
            BaseValue = baseValue;
            TraitValue = traitValue;
        }

        public Trait Condition { get; }

        public HitId HitId { get; }

        public int BaseValue { get; }

        public int TraitValue { get; }
    }
}
