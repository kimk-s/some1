using System;

namespace Some1
{
    public static class EnumForUnity
    {
        public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum
        {
#if UNITY
            return (TEnum[])Enum.GetValues(typeof(TEnum));
#else
            return Enum.GetValues<TEnum>();
#endif
        }

        public static bool IsDefined<TEnum>(TEnum x) where TEnum : struct, Enum
        {
#if UNITY
            return Enum.IsDefined(typeof(TEnum), x);
#else
            return Enum.IsDefined(x);
#endif
        }
    }
}
