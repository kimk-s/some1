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
    internal sealed class PlayerGameArgsGroup : IPlayerGameArgsGroup, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly Dictionary<GameMode, PlayerGameArgs> _all;
        private readonly GameArgsGroupData _save;
        private PlayerGameArgs _selected = null!;

        internal PlayerGameArgsGroup()
        {
            _all = EnumForUnity.GetValues<GameMode>()
                .Select(x => new PlayerGameArgs(x))
                .ToDictionary(x => x.Id, x => x);
            All = _all.ToDictionary(x => x.Key, x => (IPlayerGameArgs)x.Value);
            _sync = new SyncArraySource(_all.Values.OrderBy(x => x.Id));
            _save = new()
            {
                Items = new GameArgsData[_all.Count]
            };

            Select(GameMode.Challenge);
        }

        public IReadOnlyDictionary<GameMode, IPlayerGameArgs> All { get; }

        public IPlayerGameArgs Selected => _selected;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Load(GameArgsGroupData? data)
        {
            if (data is null)
            {
                Select(GameMode.Adventure);
                return;
            }

            foreach (var item in data.Items)
            {
                var id = (GameMode)item.Id;
                _all[id].Load(item);
            }

            Select((GameMode)data.SelectedItemId);
        }

        internal GameArgsGroupData Save()
        {
            var data = _save;

            data.SelectedItemId = (int)Selected.Id;

            int i = 0;
            foreach (var item in _all.Values)
            {
                data.Items[i++] = item.Save();
            }

            return data;
        }

        internal void Select(GameMode id)
        {
            if (_selected?.Id == id)
            {
                _selected.IsSelected = true;
                return;
            }

            if (_selected is not null)
            {
                _selected.IsSelected = false;
            }

            if (!_all.TryGetValue(id, out var item))
            {
                return;
            }

            _selected = item;
            _selected.IsSelected = true;
        }

        internal bool TrySetScore(GameMode id, int score)
        {
            if (!_all.TryGetValue(id, out var item))
            {
                return false;
            }

            if (score <= item.Score)
            {
                return false;
            }

            item.Score = score;
            return true;
        }

        internal void Reset()
        {
            foreach (var item in _all.Values)
            {
                item.Reset();
            }
            Select(GameMode.Challenge);
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
    }
}
