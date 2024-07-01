namespace Some1.Play.Info
{
    public readonly struct NonPlayerTiming
    {
        public NonPlayerTiming(float dueTime, float periodTime)
        {
            DueTime = dueTime;
            PeriodTime = periodTime;
        }

        public float DueTime { get; }
        public float PeriodTime { get; }
    }
}
