using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectShiftFront
    {
        ReadOnlyReactiveProperty<Shift?> Shift { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
        ReadOnlyReactiveProperty<float> Height { get; }
        ReadOnlyReactiveProperty<float> Cycle { get; }
        ReadOnlyReactiveProperty<float> SecondaryCycles { get; }
    }
}
