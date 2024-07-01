using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerObjectSpecialtyFront
    {
        ReadOnlyReactiveProperty<SpecialtyPacket?> Specialty { get; }
    }
}
