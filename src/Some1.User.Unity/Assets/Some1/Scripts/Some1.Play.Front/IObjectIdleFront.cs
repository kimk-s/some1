using R3;

namespace Some1.Play.Front
{
    public interface IObjectIdleFront
    {
        ReadOnlyReactiveProperty<bool> Idle { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
        ReadOnlyReactiveProperty<float> Cycle { get; }
    }
}
