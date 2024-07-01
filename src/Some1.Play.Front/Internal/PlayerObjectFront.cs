using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerObjectFront : IPlayerObjectFront, ISyncDestination, ISyncPolatable
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<int> _id = new();
        private readonly NullableUnmanagedParticleDestination<CharacterId> _characterId = new();
        private readonly ObjectBattleFront _battle = new();
        private readonly ObjectAliveFront _alive;
        private readonly PlayerObjectCastFront _cast;
        private readonly PlayerObjectSpecialtyGroupFront _specialties;
        private readonly PlayerTakeStuffGroupFront _takeStuffs;
        private readonly Dictionary<EnergyId, ObjectEnergyFront> _energies;
        private readonly ObjectTransformFront _transform;
        private readonly PlayerObjectTraitFront _trait = new();
        private readonly UnmanagedParticleDestination<byte> _team = new();

        internal PlayerObjectFront(
            ISyncTime syncFrame,
            CharacterAliveInfoGroup aliveInfos,
            CharacterCastInfoGroup castInfos,
            IRegionGroupFront regions)
        {
            _transform = new(syncFrame, _id.Value);
            _energies = EnumForUnity.GetValues<EnergyId>().ToDictionary(
                x => x,
                x => new ObjectEnergyFront(x));
            Energies = _energies.Values.Select(x => (IObjectEnergyFront)x).ToDictionary(x => x.Id);
            _cast = new(syncFrame, castInfos, CharacterId);
            _specialties = new();
            _takeStuffs = new();
            Region = new PlayerObjectRegionFront(Transform, regions);
            _alive = new(syncFrame, aliveInfos, CharacterId);

            _sync = new(
                _id,
                _characterId,
                _battle,
                _alive,
                _cast,
                _specialties,
                _takeStuffs,
                new SyncArrayDestination(_energies.Values.OrderBy(x => x.Id)),
                _transform,
                _trait,
                _team);
        }

        public ReadOnlyReactiveProperty<int> Id => _id.Value;

        public ReadOnlyReactiveProperty<CharacterId?> CharacterId => _characterId.Value;

        public IObjectBattleFront Battle => _battle;

        public IObjectAliveFront Alive => _alive;

        public IPlayerObjectCastFront Cast => _cast;

        public IPlayerObjectSpecialtyGroupFront Specialties => _specialties;

        public IPlayerTakeStuffGroupFront TakeStuffs => _takeStuffs;

        public IReadOnlyDictionary<EnergyId, IObjectEnergyFront> Energies { get; }

        public IObjectTransformFront Transform => _transform;

        public IPlayerObjectRegionFront Region { get; }

        public IPlayerObjectTraitFront Trait => _trait;

        public ReadOnlyReactiveProperty<byte> Team => _team.Value;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Extrapolate()
        {
            _sync.Extrapolate();
        }

        public void Interpolate(float time)
        {
            _sync.Interpolate(time);
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _sync.Read(ref reader, mode);
        }

        public void Reset()
        {
            _sync.Reset();
        }

        internal void Update(float deltaSeconds)
        {
            _transform.Update(deltaSeconds);
        }
    }
}
