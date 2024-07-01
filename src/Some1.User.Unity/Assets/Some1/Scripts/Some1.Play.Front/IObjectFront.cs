using System.Collections.Generic;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IObjectFront
    {
        ReadOnlyReactiveProperty<int> Id { get; }
        ReadOnlyReactiveProperty<bool> Active { get; }
        IObjectCharacterFront Character { get; }
        IObjectBattleFront Battle { get; }
        IObjectAliveFront Alive { get; }
        IObjectIdleFront Idle { get; }
        IObjectShiftFront Shift { get; }
        IObjectCastFront Cast { get; }
        IObjectWalkFront Walk { get; }
        IReadOnlyList<IObjectBuffFront> Buffs { get; }
        IReadOnlyDictionary<BoosterId, IObjectBoosterFront> Boosters { get; }
        IObjectTakeStuffGroupFront TakeStuffs { get; }
        IObjectGiveStuffFront GiveStuff { get; }
        IObjectHitGroupFront Hits { get; }
        IReadOnlyDictionary<EnergyId, IObjectEnergyFront> Energies { get; }
        IObjectTransformFront Transform { get; }
        ReadOnlyReactiveProperty<SkinId?> SkinId { get; }
        IObjectEmojiFront Emoji { get; }
        IReadOnlyList<IObjectLikeFront> Likes { get; }
        IObjectPropertiesFront Properties { get; }
        ReadOnlyReactiveProperty<ObjectRelation?> Relation { get; }
    }
}
