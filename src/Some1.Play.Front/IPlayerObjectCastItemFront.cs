using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerObjectCastItemFront
    {
        CastId Id { get; }
        ReadOnlyReactiveProperty<float> LoadWave { get; }
        ReadOnlyReactiveProperty<float> NormalizedLoadWave { get; }
        ReadOnlyReactiveProperty<int> LoadCount { get; }
        ReadOnlyReactiveProperty<int> MaxLoadCount { get; }
        ReadOnlyReactiveProperty<bool> AnyLoadCount { get; }
        ReadOnlyReactiveProperty<float> Delay { get; }
        ReadOnlyReactiveProperty<CharacterCastInfo?> Info { get; }
    }
}
