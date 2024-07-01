using Some1.Play.Info;
using R3;

namespace Some1.Play.Core
{
    public interface IObjectSpecialty
    {
        int Index { get; }
        ReadOnlyReactiveProperty<SpecialtyId?> Id { get; }
        ReadOnlyReactiveProperty<int> Number { get; }
        ReadOnlyReactiveProperty<bool> IsPinned { get; }
        ReadOnlyReactiveProperty<int> Token { get; }
    }
}
