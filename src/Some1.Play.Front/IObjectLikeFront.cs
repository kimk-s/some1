using R3;

namespace Some1.Play.Front
{
    public interface IObjectLikeFront
    {
        ReadOnlyReactiveProperty<bool> Value { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
    }
}
