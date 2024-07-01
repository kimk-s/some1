namespace Some1.Play.Info
{
    public enum SectionType
    {
        Town,
        Yard,
        Field,
        Empty,
        End
    }

    public static class SectionTypeExtensions
    {
        public static bool IsNonBattle(this SectionType x) => !x.IsBattle();

        public static bool IsBattle(this SectionType x) => x switch
        {
            SectionType.Town => false,
            _ => true
        };
    }
}
