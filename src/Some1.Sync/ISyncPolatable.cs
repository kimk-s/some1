namespace Some1.Sync
{
    public interface ISyncPolatable
    {
        void Extrapolate();
        void Interpolate(float time);
    }
}
