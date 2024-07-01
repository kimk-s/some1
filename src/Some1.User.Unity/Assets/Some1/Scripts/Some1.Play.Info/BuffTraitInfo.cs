namespace Some1.Play.Info
{
    public sealed class BuffTraitInfo
    {
        public BuffTraitInfo(Trait condition)
        {
            Condition = condition;
        }

        public Trait Condition { get; }
    }
}
