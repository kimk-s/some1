namespace Some1.Play.Info.Alpha
{
    public static class AlphaSpaceInfoFactory
    {
        public static SpaceInfo Create(AlphaPlayInfoEnvironment environment) => new(
            environment.IsProduction() ? new(360, 720) : new(180, 360),
            4,
            environment.IsProduction() ? new(360, 360) : new(180, 180),
            environment.IsProduction() ? 120 : 60,
            2,
            4,
            4,
            16,
            8,
            environment.IsProduction() ? 8 : 4);
    }
}
