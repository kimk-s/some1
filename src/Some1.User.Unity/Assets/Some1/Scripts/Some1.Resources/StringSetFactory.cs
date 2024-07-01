using System;
using Some1.Resources;

namespace Some1.Localizing
{
    internal static partial class StringSetFactory
    {
        public static StringSet Create(Culture culture)
        {
            return culture switch
            {
                Culture.en_US => Create_en_US(),
                Culture.ko_KR => Create_ko_KR(),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
