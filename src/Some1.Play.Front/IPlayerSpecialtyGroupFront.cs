using System.Collections.Generic;
using R3;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public interface IPlayerSpecialtyGroupFront
    {
        IReadOnlyDictionary<SpecialtyId, IPlayerSpecialtyFront> All { get; }
        ReadOnlyReactiveProperty<Leveling> Star { get; }
    }
}
