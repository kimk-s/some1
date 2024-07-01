using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using R3;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerCharacter : IPlayerCharacter, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<bool> _isPicked = new();
        private readonly ReactiveProperty<bool> _isSelected = new();
        private readonly ReactiveProperty<bool> _isRandomSkin = new();
        private readonly ReactiveProperty<int> _exp = new();
        private readonly ReactiveProperty<DateTime> _expUtc = new();
        private readonly CharacterInfo _info;
        private readonly Dictionary<SkinId, PlayerCharacterSkin> _skins;
        private readonly CharacterData _save;
        private PlayerCharacterSkin _electedSkin = null!;
        private PlayerCharacterSkin _selectedSkin = null!;
        private bool _isPremium;

        internal PlayerCharacter(CharacterInfo info, IReadOnlyDictionary<SkinId, CharacterSkinInfo> skinInfos)
        {
            _info = info;
            Id = _info.Id;

            _skins = skinInfos.Values.Select(x => new PlayerCharacterSkin(x)).ToDictionary(
                x => x.Id,
                x => x);
            Skins = _skins.ToDictionary(
                x => x.Key,
                x => (IPlayerCharacterSkin)x.Value);

            _sync = new SyncArraySource(
                _isPicked.ToUnmanagedParticleSource(),
                _isSelected.ToUnmanagedParticleSource(),
                _isRandomSkin.ToUnmanagedParticleSource(),
                new SyncArraySource(_skins.Values),
                _exp.ToUnmanagedParticleSource(),
                _expUtc.ToUnmanagedParticleSource());

            _save = new()
            {
                Skins = new CharacterSkinData[_skins.Count]
            };

            UpdateIsUnlocked();
            SelectSkin(SkinId.Skin0);
            SetSkinIsRandomSelected(SkinId.Skin0, true);
        }

        public CharacterId Id { get; }

        public bool IsUnlocked { get; private set; }

        public bool IsPicked
        {
            get => _isPicked.Value;

            internal set
            {
                _isPicked.Value = value;
                UpdateIsUnlocked();
            }
        }

        public bool IsSelected { get => _isSelected.Value; internal set => _isSelected.Value = value; }

        public bool IsRandomSkin
        {
            get => _isRandomSkin.Value;

            internal set
            {
                if (IsRandomSkin == value)
                {
                    return;
                }

                _isRandomSkin.Value = value;

                if (value)
                {
                    ElectSkin(GetRandomRandomSelectedSkin());
                }
                else
                {
                    ElectSkin(_selectedSkin);
                }
            }
        }

        public int Exp
        {
            get => _exp.Value;

            private set
            {
                value = Math.Clamp(value, 0, int.MaxValue);
                if (value == Exp)
                {
                    return;
                }

                _exp.Value = value;

                Star = checked(new Leveling(value, PlayConst.CharacterStarLeveling_MaxLevel, PlayConst.CharacterStarLeveling_StepFactor, LevelingMethod.Plain).Level);
            }
        }

        public int Star { get; private set; }

        public IReadOnlyDictionary<SkinId, IPlayerCharacterSkin> Skins { get; }

        public IPlayerCharacterSkin ElectedSkin => _electedSkin;

        internal bool IsPremium
        {
            get => _isPremium;

            set
            {
                if (_isPremium == value)
                {
                    return;
                }
                _isPremium = value;
                UpdateIsUnlocked();

                foreach (var item in _skins.Values)
                {
                    item.IsPremium = value;
                }

                if (!value)
                {
                    if (!SelectedSkin.IsUnlocked)
                    {
                        SelectSkin(SkinId.Skin0);

                        foreach (var item in _skins.Values)
                        {
                            SetSkinIsRandomSelected(item.Id, item.Id == SkinId.Skin0);
                        }
                    }
                }
            }
        }

        internal IPlayerCharacterSkin SelectedSkin => _selectedSkin;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal CharacterData Save()
        {
            var save = _save;

            save.Id = (int)Id;
            save.IsPicked = IsPicked;
            save.IsRandomSkin = IsRandomSkin;
            save.Exp = Exp;
            save.ExpUtc = _expUtc.CurrentValue;
            save.SelectedSkinId = (int)SelectedSkin.Id;

            int i = 0;
            foreach (var item in _skins.Values)
            {
                save.Skins[i++] = new(
                    (int)item.Id,
                    item.IsRandomSelected);
            }

            return save;
        }

        internal void SelectSkin(SkinId id)
        {
            if (!_skins.TryGetValue(id, out var skin))
            {
                return;
            }

            if (skin.IsSelected || !skin.IsUnlocked)
            {
                return;
            }

            if (_selectedSkin is not null)
            {
                _selectedSkin.IsSelected = false;
            }
            _selectedSkin = skin;
            _selectedSkin.IsSelected = true;

            if (!IsRandomSkin)
            {
                ElectSkin(skin);
            }
        }

        internal void SetSkinIsRandomSelected(SkinId id, bool value)
        {
            if (!_skins.TryGetValue(id, out var skin))
            {
                return;
            }

            if (skin.IsRandomSelected == value || !skin.IsUnlocked)
            {
                return;
            }

            if (!value)
            {
                if (GetRandomSelectedSkinCount() == 1)
                {
                    return;
                }
            }

            skin.IsRandomSelected = value;

            if (IsRandomSkin)
            {
                if (value)
                {
                    ElectSkin(skin);
                }
                else
                {
                    if (skin.IsElected)
                    {
                        ElectSkin(GetRandomRandomSelectedSkin());
                    }
                }
            }
        }

        internal void AddExp(int value, DateTime utc)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (value == 0)
            {
                return;
            }

            Exp += value;
            _expUtc.Value = utc;
        }

        internal void Reset()
        {
            IsPicked = false;
            IsSelected = false;
            _isRandomSkin.Value = false;
            foreach (var item in _skins.Values)
            {
                item.Reset();
            }
            SelectSkin(SkinId.Skin0);
            SetSkinIsRandomSelected(SkinId.Skin0, true);
            Exp = 0;
            _expUtc.Value = DateTime.MinValue;
            IsPremium = false;
        }

        private void UpdateIsUnlocked()
        {
            IsUnlocked = IsPicked || IsPremium;
        }

        private void ElectSkin(PlayerCharacterSkin skin)
        {
            if (skin.IsElected)
            {
                return;
            }

            if (_electedSkin is not null)
            {
                _electedSkin.IsElected = false;
            }
            _electedSkin = skin;
            _electedSkin.IsElected = true;
        }

        private int GetRandomSelectedSkinCount()
        {
            int i = 0;
            foreach (var item in _skins.Values)
            {
                if (item.IsRandomSelected)
                {
                    i++;
                }
            }
            return i;
        }

        private PlayerCharacterSkin GetRandomRandomSelectedSkin()
        {
            int randomIndex = RandomForUnity.Next(GetRandomSelectedSkinCount());

            int i = 0;
            foreach (var item in _skins.Values)
            {
                if (item.IsRandomSelected)
                {
                    if (i == randomIndex)
                    {
                        return item;
                    }

                    i++;
                }
            }
            throw new InvalidOperationException();
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        public void Dispose()
        {
            _sync.Dispose();
        }

        internal void ReElectIfRandomSkin()
        {
            if (!IsRandomSkin)
            {
                return;
            }

            ElectSkin(GetRandomRandomSelectedSkin());
        }
    }
}
