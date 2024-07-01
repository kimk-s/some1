using System.Drawing;

namespace Some1.Play.Info
{
    public static class MixableExtensions
    {
        public static float Mix(this Mixable<float> mixable, float other)
            => mixable.Value + (mixable.IsMix ? other : 0);

        public static SizeF Mix(this Mixable<SizeF> mixable, SizeF other)
            => mixable.Value + (mixable.IsMix ? other : SizeF.Empty);

        public static AreaType Mix(this Mixable<AreaType> mixable, AreaType other)
            => mixable.IsMix
            ? (mixable.Value == AreaType.Rectangle || other == AreaType.Rectangle ? AreaType.Rectangle : AreaType.Circle)
            : mixable.Value;
    }
}
