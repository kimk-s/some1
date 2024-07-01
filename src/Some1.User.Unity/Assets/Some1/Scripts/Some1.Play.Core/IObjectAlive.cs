using R3;

namespace Some1.Play.Core
{
    public interface IObjectAlive
    {
        ReadOnlyReactiveProperty<bool> Alive { get; }
        IObjectCycles Cycles { get; }
    }
}
