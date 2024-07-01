using R3;

namespace Some1.Play.Front
{
    public interface IObjectGiveStuffFront
    {
        ReadOnlyReactiveProperty<bool> IsTaken { get; }
    }
}
