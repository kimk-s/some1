using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectPropertiesFront
    {
        ReadOnlyReactiveProperty<byte> Team { get; }
        ReadOnlyReactiveProperty<ObjectPlayer?> Player { get; }
    }
}
