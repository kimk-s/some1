namespace Some1.Sync
{
    public interface ISyncTime
    {
        int FrameCount { get; }
        FloatWave DeltaTime { get; }
        FloatWave FPS { get; }
    }
}
