using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectBuffFront
    {
        ReadOnlyReactiveProperty<BuffSkinId?> Id { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
    }
}
