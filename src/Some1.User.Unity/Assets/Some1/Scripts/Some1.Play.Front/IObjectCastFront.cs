using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectCastFront
    {
        ReadOnlyReactiveProperty<CastPacket?> Cast { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
        ReadOnlyReactiveProperty<float> Cycle { get; }
    }
}
