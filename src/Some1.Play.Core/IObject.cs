using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObject
    {
        int Id { get; }
        bool IsResetReserved { get; }
        IObjectCharacter Character { get; }
        IObjectBattle Battle { get; }
        IObjectAlive Alive { get; }
        IObjectIdle Idle { get; }
        IObjectShift Shift { get; }
        IObjectCast Cast { get; }
        IObjectWalk Walk { get; }
        IReadOnlyList<IObjectBuff> Buffs { get; }
        IReadOnlyDictionary<BoosterId, IObjectBooster> Boosters { get; }
        IReadOnlyList<IObjectSpecialty> Specialties { get; }
        IObjectTakeTakeStuffGroup TakeStuffs { get; }
        IObjectGiveStuff GiveStuff { get; }
        IObjectHitReached HitReached { get; }
        IReadOnlyList<IObjectHit> Hits { get; }
        IReadOnlyDictionary<StatId, IObjectStat> Stats { get; }
        IObjectEnergyReached EnergyReached { get; }
        IReadOnlyDictionary<EnergyId, IObjectEnergy> Energies { get; }
        IObjectHierarchy Hierarchy { get; }
        IObjectTransfer Transfer { get; }
        IObjectTransform Transform { get; }
        IObjectSkin Skin { get; }
        IObjectEmoji Emoji { get; }
        IObjectLike Like { get; }
        IObjectRegion Region { get; }
        IObjectTrait Trait { get; }
        IObjectProperties Properties { get; }
        IControl Control { get; }

        bool Look(Looker looker);
        bool GetStuffAcquired(int objectId);
    }
}
