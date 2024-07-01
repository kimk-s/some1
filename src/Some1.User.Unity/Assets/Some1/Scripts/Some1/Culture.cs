using System;

namespace Some1
{
    public enum Culture
    {
        None,
        en_US,
        ko_KR,
    }

    public static class CultureExtensions
    {
        public static string GetName(this Culture culture) => culture switch
        {
            Culture.en_US => "en-US",
            Culture.ko_KR => "ko-KR",
            Culture.None => "-",
            _ => throw new InvalidOperationException()
        };

        public static string GetLongName(this Culture culture) => $"{culture.GetEnglishName()} ({culture.GetNativeName()})";

        public static string GetEnglishName(this Culture culture) => culture switch
        {
            Culture.en_US => "English",
            Culture.ko_KR => "Korean",
            Culture.None => "-",
            _ => throw new InvalidOperationException(),
        };

        public static string GetNativeName(this Culture culture) => culture switch
        {
            Culture.en_US => "English",
            Culture.ko_KR => "한국어",
            Culture.None => "-",
            _ => throw new InvalidOperationException(),
        };
    }
}
