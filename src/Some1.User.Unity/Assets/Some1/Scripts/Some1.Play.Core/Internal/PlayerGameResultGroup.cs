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
    internal sealed class PlayerGameResultGroup : IPlayerGameResultGroup, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly PlayerGameResult[] _all;
        private readonly GameResultGroupData _saveData;

        internal PlayerGameResultGroup()
        {
            _all = Enumerable.Range(0, PlayConst.PlayerGameResultCount)
                .Select(_ => new PlayerGameResult())
                .ToArray();
            _sync = new SyncArraySource(_all);
            _saveData = new()
            {
                Items = new GameResultData[_all.Length]
            };
        }

        public IReadOnlyList<IPlayerGameResult> All => _all;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Load(GameResultGroupData? data)
        {
            if (data is null)
            {
                return;
            }

            int count = Math.Min(data.Items.Length, _all.Length);
            for (int i = 0; i < count; i++)
            {
                _all[i].Load(data.Items[i]);
            }
        }

        internal GameResultGroupData Save()
        {
            var data = _saveData;

            int count = _all.Length;
            for (int i = 0; i < count; i++)
            {
                data.Items[i] = (GameResultData)_all[i].Save();
            }

            return data;
        }

        public void ClearDirty()
        {
            _sync.ClearDirty();
        }

        public void Dispose()
        {
            _sync.Dispose();
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            _sync.Write(ref writer, mode);
        }

        internal void Add(GameResult result)
        {
            GetNullOrOldestResult().Set(result);
        }

        internal void Reset()
        {
            foreach (var item in _all)
            {
                item.Reset();
            }
        }

        private PlayerGameResult GetNullOrOldestResult()
        {
            PlayerGameResult result = _all[0];

            foreach (var item in _all)
            {
                if (item.Result is null)
                {
                    result = item;
                    break;
                }

                if (item.Result.Value.EndTime < result.Result!.Value.EndTime)
                {
                    result = item;
                }
            }

            return result;
        }
    }
}
