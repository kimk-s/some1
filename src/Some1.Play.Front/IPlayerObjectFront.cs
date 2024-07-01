using System.Collections.Generic;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerObjectFront
    {
        ReadOnlyReactiveProperty<int> Id { get; }
        ReadOnlyReactiveProperty<CharacterId?> CharacterId { get; }
        IObjectBattleFront Battle { get; }
        IObjectAliveFront Alive { get; }
        IPlayerObjectCastFront Cast { get; }
        IPlayerObjectSpecialtyGroupFront Specialties { get; }
        IPlayerTakeStuffGroupFront TakeStuffs { get; }
        IReadOnlyDictionary<EnergyId, IObjectEnergyFront> Energies { get; }
        IObjectTransformFront Transform { get; }
        IPlayerObjectRegionFront Region { get; }
        IPlayerObjectTraitFront Trait { get; }
        ReadOnlyReactiveProperty<byte> Team { get; }
    }
}
