using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerUnaryFront
    {
        ReadOnlyReactiveProperty<UnaryResult?> Result { get; }
    }
}
