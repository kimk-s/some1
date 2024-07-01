using System;

namespace Some1.Play.Info
{
    public enum StatId
    {
        Accel,
        Offense,
        Defense,
        Health,
        Mana,
        StunCast,
        StunWalk,
    }

    public static class StatHelper
    {
        public static int CalculateDamage(int @base, int offenceStat, int defenseStat)
        {
            //defenseStat = Math.Max(0, defenseStat);
            //return ToInt(Calculate((float)@base, offenceStat) * (100f / (100 + defenseStat)));

            defenseStat = Math.Clamp(defenseStat, 0, 9);
            return ToInt(Calculate((float)@base, offenceStat) * (1 - defenseStat * 0.1f));
        }

        public static int Calculate(int @base, int stat)
        {
            return ToInt(Calculate((float)@base, stat));
        }

        public static float Calculate(float @base, int stat)
        {
            return @base * (1 + 0.1f * stat);
        }

        private static int ToInt(float value) => (int)MathF.Truncate(value);
    }
}
