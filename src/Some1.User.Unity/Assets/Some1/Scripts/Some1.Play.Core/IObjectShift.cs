using R3;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectShift
    {
        ReadOnlyReactiveProperty<Shift?> Shift { get; }
        IObjectCycles Cycles { get; }
    }
}
