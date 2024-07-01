using R3;

namespace Some1.Play.Front
{
    public interface IObjectAliveFront
    {
        ReadOnlyReactiveProperty<bool> Alive { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
        ReadOnlyReactiveProperty<float> Cycle { get; }
    }
}
