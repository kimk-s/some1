using Some1.Play.Info;
using R3;

namespace Some1.Play.Core
{
    public interface IObjectCastItem
    {
        CastId Id { get; }
        ReadOnlyReactiveProperty<FloatWave> LoadWave { get; }
        int LoadCount { get; }
        int MaxLoadCount { get; }
        bool AnyLoadCount { get; }
        ObjectTarget ObjectTarget { get; }

        bool CanConsume();
    }
}
