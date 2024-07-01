using R3;

namespace Some1.Play.Core
{
    public interface IObjectTransform
    {
        ReadOnlyReactiveProperty<Vector2Wave> Position { get; }
        ReadOnlyReactiveProperty<float> Rotation { get; }
    }
}
