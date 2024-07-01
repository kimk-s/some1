namespace Some1.Play.Info
{
    public readonly struct TriggerConditionInfo
    {
        public TriggerConditionInfo(Trait trait, Trait nextTrait, float probability)
        {
            Trait = trait;
            NextTrait = nextTrait;
            Probability = probability;
        }

        public static TriggerConditionInfo All { get; } = new(Trait.All, Trait.All, 1);

        public static TriggerConditionInfo None { get; } = new(Trait.None, Trait.None, 0);

        public Trait Trait { get; }
        public Trait NextTrait { get; }
        public float Probability { get; }
    }
}
