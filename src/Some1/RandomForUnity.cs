using System;

namespace Some1
{
    public static class RandomForUnity
    {
#if UNITY
        private static readonly Random s_random = new Random();
#endif

        public static float NextSingle()
        {
#if UNITY
            return (float)s_random.NextDouble();
#else
            return Random.Shared.NextSingle();
#endif
        }

        public static int Next(int maxValue)
        {
#if UNITY
            return s_random.Next(maxValue);
#else
            return Random.Shared.Next(maxValue);
#endif
        }
    }
}
