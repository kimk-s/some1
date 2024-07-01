namespace Some1.Play.Info
{
    public readonly struct NonPlayerDensity
    {
        public NonPlayerDensity(float value, bool countCeiling, bool spread)
        {
            Value = value;
            CountCeiling = countCeiling;
            Spread = spread;
        }

        public float Value { get; }
        public bool CountCeiling { get; }
        public bool Spread { get; }
    }
}
