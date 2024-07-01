using System;

namespace Some1.Play.Info
{
    [Flags]
    public enum HitId : byte
    {
        Default = 0,
        Recovery = 1 << 0,
        Unique = 1 << 1,
        FireAttribute = 1 << 2,
        PoisonAttribute = 1 << 3,
    }

    public static class HitIdExtensions
    {
        public static bool IsDamage(this HitId x) => !x.IsRecovery();

        public static bool IsRecovery(this HitId x) => x.HasFlag(HitId.Recovery);

        public static bool IsUnique(this HitId x) => x.HasFlag(HitId.Unique);

        public static bool HasAttribute(this HitId x, HitAttribute attribute) => x.HasFlag(attribute.ToHitId());

        private static HitId ToHitId(this HitAttribute attribute) => attribute switch
        {
            HitAttribute.Fire => HitId.FireAttribute,
            HitAttribute.Poison => HitId.PoisonAttribute,
            _ => throw new ArgumentOutOfRangeException(nameof(attribute))
        };
    }
}
