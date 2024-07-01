namespace Some1.Play.Client
{
    public interface IPlayStreamer
    {
        delegate DoubleWave GetTimeDelegate();
        delegate bool ReadDelegate();
        delegate float GetReadableDeltaTimeDelegate(float maxDeltaTime);
        delegate void InterpolateDelegate(float time);
        delegate void SetLocalTimeScaleDelegate(float timeScale);

        void Setup(
            GetTimeDelegate getTime,
            ReadDelegate read,
            GetReadableDeltaTimeDelegate getReadableDeltaTime,
            InterpolateDelegate interpolate,
            SetLocalTimeScaleDelegate setLocalTimeScale);
        void Update(float localDeltaSeconds);
        void ResetState();
    }
}
