using System;

namespace Some1.Play.Info.Alpha
{
    public enum AlphaPlayInfoEnvironment
    {
        Development,
        Staging,
        Production
    }

    public static class AlphaPlayInfoEnvironmentExtensions
    {
        public static bool IsDevlopment(this AlphaPlayInfoEnvironment environment) => environment == AlphaPlayInfoEnvironment.Development;
        public static bool IsStaging(this AlphaPlayInfoEnvironment environment) => environment == AlphaPlayInfoEnvironment.Staging;
        public static bool IsProduction(this AlphaPlayInfoEnvironment environment) => environment == AlphaPlayInfoEnvironment.Production;
    }

    public static class AlphaPlayInfoEnvironmentParser
    {
        public static AlphaPlayInfoEnvironment Parse(string value)
        {
            if (!Enum.TryParse<AlphaPlayInfoEnvironment>(value, out var result))
            {
                return AlphaPlayInfoEnvironment.Development;
            }

            return result;
        }
    }
}
