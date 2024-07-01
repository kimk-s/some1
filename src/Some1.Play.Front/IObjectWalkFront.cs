using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectWalkFront
    {
        ReadOnlyReactiveProperty<Walk?> Walk { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
        ReadOnlyReactiveProperty<float> Cycle { get; }
    }
}
