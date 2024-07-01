using System.Numerics;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectTransformFront
    {
        ReadOnlyReactiveProperty<Vector2> Position { get; }
        ReadOnlyReactiveProperty<float> Rotation { get; }
    }
}
