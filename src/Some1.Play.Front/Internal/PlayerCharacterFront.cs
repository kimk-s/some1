using System;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Destinations;

namespace Some1.Play.Front.Internal
{
    internal sealed class PlayerCharacterFront : IPlayerCharacterFront, ISyncDestination
    {
        private readonly SyncArrayDestination _sync;
        private readonly UnmanagedParticleDestination<bool> _isPicked = new();
        private readonly UnmanagedParticleDestination<bool> _isSelected = new();
        private readonly UnmanagedParticleDestination<bool> _isRandomSkin = new();
        private readonly UnmanagedParticleDestination<int> _exp = new();
        private readonly UnmanagedParticleDestination<DateTime> _expUtc = new();
        private readonly Dictionary<SkinId, PlayerCharacterSkinFront> _skins;

        internal PlayerCharacterFront(
            CharacterInfo info,
            CharacterSkillInfoGroup skillInfos,
            CharacterSkillPropInfoGroup skillPropInfos,
            CharacterEnergyInfoGroup energyInfos,
            CharacterSkinInfoGroup skinInfos,
            IPlayerPremiumFront premium,
            ITimeFront time)
        {
            Id = info.Id;
            Role = info.Role;
            Season = info.Season;
            WalkSpeed = WalkSpeedHelper.Get(info.Walk?.Speed ?? throw new InvalidOperationException());
            Health = energyInfos.ById[new(Id, EnergyId.Health)].Value;
            Skills = skillInfos.GroupByCharacterId[Id].Values
                .Select(x => new PlayerCharacterSkillFront(x, skillPropInfos))
                .ToArray();

            IsUnlocked = _isPicked.Value
                .CombineLatest(premium.IsPremium, (a, b) => a || b)
                .ToReadOnlyReactiveProperty();
            _skins = skinInfos.GroupByCharacterId[Id].Values
                .Select(x => new PlayerCharacterSkinFront(x, premium))
                .ToDictionary(x => x.Id.Skin);
            Skins = _skins.Values
                .Select(x => (IPlayerCharacterSkinFront)x)
                .ToDictionary(x => x.Id.Skin);
            Observable<IPlayerCharacterSkinFront> aa = Observable
                .Merge(Skins.Values.Select(x => x.IsElected.Where(x => x).Select(_ => x)));
            ElectedSkin = Observable
                .Merge(Skins.Values.Select(x => x.IsElected.Where(x => x).Select(_ => x)))!
                .ToReadOnlyReactiveProperty();
            SelectedSkin = Observable
                .Merge(Skins.Values.Select(x => x.IsSelected.Where(x => x).Select(_ => x)))!
                .ToReadOnlyReactiveProperty();
            Star = Exp
                .Select(x => new Leveling(x, PlayConst.CharacterStarLeveling_MaxLevel, PlayConst.CharacterStarLeveling_StepFactor, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();
            ExpTimeAgo = _expUtc.Value
                .CombineLatest(
                    time.UtcNow,
                    (expUtc, utcNow) => expUtc == DateTime.MinValue ? (TimeSpan?)null : (utcNow - expUtc))
                .ToReadOnlyReactiveProperty();

            _sync = new SyncArrayDestination(
                _isPicked,
                _isSelected,
                _isRandomSkin,
                new SyncArrayDestination(_skins.Values.OrderBy(x => x.Id.Skin)),
                _exp,
                _expUtc);
        }

        public CharacterId Id { get; }

        public CharacterRole? Role { get; }

        public SeasonId? Season { get; }

        public WalkSpeed WalkSpeed { get; }

        public int Health { get; }

        public IReadOnlyList<IPlayerCharacterSkillFront> Skills { get; }

        public ReadOnlyReactiveProperty<bool> IsUnlocked { get; }

        public ReadOnlyReactiveProperty<bool> IsPicked => _isPicked.Value;

        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected.Value;

        public ReadOnlyReactiveProperty<bool> IsRandomSkin => _isRandomSkin.Value;

        public IReadOnlyDictionary<SkinId, IPlayerCharacterSkinFront> Skins { get; }

        public ReadOnlyReactiveProperty<IPlayerCharacterSkinFront?> ElectedSkin { get; }

        public ReadOnlyReactiveProperty<IPlayerCharacterSkinFront?> SelectedSkin { get; }

        public ReadOnlyReactiveProperty<int> Exp => _exp.Value;

        public ReadOnlyReactiveProperty<TimeSpan?> ExpTimeAgo { get; }

        public ReadOnlyReactiveProperty<Leveling> Star { get; }

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            _sync.Read(ref reader, mode);
        }

        public void Reset()
        {
            _sync.Reset();
        }
    }
}
