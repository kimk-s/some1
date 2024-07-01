using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectTakeStuffFront
    {
        ReadOnlyReactiveProperty<Stuff?> Stuff { get; }
        ReadOnlyReactiveProperty<float> Cycles { get; }
    }
}
