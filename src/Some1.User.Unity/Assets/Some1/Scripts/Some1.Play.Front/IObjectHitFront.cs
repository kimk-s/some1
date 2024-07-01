using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectHitFront
    {
        ReadOnlyReactiveProperty<HitPacket?> Hit { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
        ReadOnlyReactiveProperty<bool> ToMe { get; }
        ReadOnlyReactiveProperty<bool> FromMe { get; }
    }
}
