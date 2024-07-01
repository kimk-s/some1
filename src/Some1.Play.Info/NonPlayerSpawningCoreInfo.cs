namespace Some1.Play.Info
{
    public sealed class NonPlayerSpawningCoreInfo
    {
        public NonPlayerSpawningCoreInfo(
            CharacterId characterId,
            NonPlayerTiming timing,
            NonPlayerRectTransform rectTransform,
            NonPlayerDensity density)
        {
            CharacterId = characterId;
            Timing = timing;
            RectTransform = rectTransform;
            Density = density;
        }

        public CharacterId CharacterId { get; }
        public NonPlayerTiming Timing { get; }
        public NonPlayerRectTransform RectTransform { get; }
        public NonPlayerDensity Density { get; }
    }
}
