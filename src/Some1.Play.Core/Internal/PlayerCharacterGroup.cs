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
    internal sealed class PlayerCharacterGroup : IPlayerCharacterGroup, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly Dictionary<CharacterId, PlayerCharacter> _all;
        private readonly PlayerTitle _title;
        private readonly Object _object;
        private readonly ITime _time;
        private readonly ReactiveProperty<DateTime> _pickTime = new();
        private readonly CharacterId[] _additionalPickIds;
        private readonly CharacterGroupData _save;
        private PlayerCharacter _selected = null!;
        private bool _isPremium;

        internal PlayerCharacterGroup(
            CharacterInfoGroup infos,
            CharacterSkinInfoGroup skinInfos,
            PlayerTitle title,
            Object @object,
            ITime time)
        {
            if (infos.WherePlayerById.Count == 0)
            {
                throw new ArgumentException("", nameof(infos));
            }

            _all = infos.WherePlayerById.ToDictionary(
                x => x.Key,
                x => new PlayerCharacter(x.Value, skinInfos.GroupByCharacterId[x.Key]));
            All = _all.ToDictionary(
                x => x.Key,
                x => (IPlayerCharacter)x.Value);
            _title = title;
            _object = @object;
            _time = time;

            _additionalPickIds = _all.Keys.Where(x => x != CharacterId.Player1).ToArray();

            _sync = new SyncArraySource(
                new SyncArraySource(_all.Values.OrderBy(x => x.Id)),
                _pickTime.ToUnmanagedParticleSource());

            _save = new()
            {
                Items = new CharacterData[_all.Count]
            };

            Pick();
        }

        public IReadOnlyDictionary<CharacterId, IPlayerCharacter> All { get; }

        public ReadOnlyReactiveProperty<DateTime> PickTime => _pickTime;

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

                foreach (var item in _all.Values)
                {
                    item.IsPremium = value;
                }

                SelectInternal(_selected.Id);
            }
        }

        internal IPlayerCharacter Selected => _selected;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Load(CharacterGroupData? data)
        {
            if (data is null)
            {
                Pick();
                return;
            }

            _all[CharacterId.Player1].IsPicked = true;
            SelectInternal(CharacterId.Player1);

            foreach (var x in data.Items)
            {
                var id = (CharacterId)x.Id;

                _all[id].IsPicked = x.IsPicked;
                SetIsRandomSkinInternal(id, x.IsRandomSkin);
                AddExp(id, x.Exp, x.ExpUtc);
                SelectSkinInternal(id, (SkinId)x.SelectedSkinId);

                foreach (var y in x.Skins)
                {
                    SetSkinIsRandomSelectedInternal(id, (SkinId)y.Id, y.IsRandomSelected);
                }
            }

            SelectInternal((CharacterId)data.SelectedItemId);
            _pickTime.Value = data.PickTime;
        }

        internal CharacterGroupData Save()
        {
            var save = _save;

            save.SelectedItemId = (int)_selected.Id;
            save.PickTime = _pickTime.Value;

            int i = 0;
            foreach (var item in _all.Values)
            {
                save.Items[i++] = item.Save();
            }

            return save;
        }

        internal void Select(CharacterId id)
        {
            if (_object.Region.Section?.Type.IsBattle() == true)
            {
                return;
            }

            SelectInternal(id);
        }

        private void SelectInternal(CharacterId id)
        {
            if (!_all.TryGetValue(id, out var character))
            {
                return;
            }

            if (!character.IsSelected && character.IsUnlocked)
            {
                if (_selected is not null)
                {
                    _selected.IsSelected = false;
                }
                _selected = character;
                _selected.IsSelected = true;
            }

            _object.SetCharacter(_selected.Id);
            _object.SetSkin(_selected.ElectedSkin.Id);
        }

        internal void SetIsRandomSkin(CharacterId id, bool value)
        {
            if (_object.Region.Section?.Type.IsBattle() == true)
            {
                return;
            }

            SetIsRandomSkinInternal(id, value);
        }

        private void SetIsRandomSkinInternal(CharacterId id, bool value)
        {
            if (!_all.TryGetValue(id, out var character))
            {
                return;
            }

            if (character.IsRandomSkin == value)
            {
                return;
            }

            character.IsRandomSkin = value;

            _object.SetSkin(_selected.ElectedSkin.Id);
        }

        internal void SelectSkin(CharacterId id, SkinId skinId)
        {
            if (_object.Region.Section?.Type.IsBattle() == true)
            {
                return;
            }

            SelectSkinInternal(id, skinId);
        }

        private void SelectSkinInternal(CharacterId id, SkinId skinId)
        {
            if (!_all.TryGetValue(id, out var character))
            {
                return;
            }

            character.SelectSkin(skinId);

            _object.SetSkin(_selected.ElectedSkin.Id);
        }

        internal void SetSkinIsRandomSelected(CharacterId id, SkinId skinId, bool value)
        {
            if (_object.Region.Section?.Type.IsBattle() == true)
            {
                return;
            }

            SetSkinIsRandomSelectedInternal(id, skinId, value);
        }

        private void SetSkinIsRandomSelectedInternal(CharacterId id, SkinId skinId, bool value)
        {
            if (!_all.TryGetValue(id, out var character))
            {
                return;
            }

            character.SetSkinIsRandomSelected(skinId, value);

            _object.SetSkin(_selected.ElectedSkin.Id);
        }

        internal void AddExp(int value)
        {
            AddExp(_selected.Id, value, _time.UtcNow);
        }

        private void AddExp(CharacterId id, int value, DateTime utc)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (!_all.TryGetValue(id, out var character))
            {
                return;
            }

            int star = character.Star;

            character.AddExp(value, utc);

            int starDiff = character.Star - star;
            if (starDiff < 0)
            {
                throw new InvalidOperationException();
            }
            else if (starDiff > 0)
            {
                _title.AddStar(starDiff);
            }
        }

        internal void Reset()
        {
            foreach (var item in _all.Values)
            {
                item.Reset();
            }
            SelectInternal(CharacterId.Player1);
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

        internal void Update()
        {
            if (_object.Region.Section?.Type.IsNonBattle() == true)
            {
                if (!_selected.IsUnlocked)
                {
                    SelectInternal(CharacterId.Player1);
                }

                if (_pickTime.Value <= _time.UtcNow)
                {
                    Pick();
                }
            }
        }

        private void Pick()
        {
            var additionalPick = _additionalPickIds.Length == 0
                ? (CharacterId?)null
                : _additionalPickIds[RandomForUnity.Next(_additionalPickIds.Length)];

            foreach (var item in _all.Values)
            {
                item.IsPicked = item.Id == CharacterId.Player1 || item.Id == additionalPick;
            }

            var x = _time.UtcNow.AddDays(1);
            _pickTime.Value = new(x.Year, x.Month, x.Day);

            SelectInternal(CharacterId.Player1);
        }

        internal void ReElectIfRandomSkin()
        {
            _selected.ReElectIfRandomSkin();

            _object.SetSkin(_selected.ElectedSkin.Id);
        }
    }
}
