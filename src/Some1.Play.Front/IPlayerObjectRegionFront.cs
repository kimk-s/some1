using R3;

namespace Some1.Play.Front
{
    public interface IPlayerObjectRegionFront
    {
        ReadOnlyReactiveProperty<IRegionSectionFront?> Section { get; }
    }
}
