using System.Buffers;
using MemoryPack;
using R3;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerGameArgs : IPlayerGameArgs, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<bool> _isSelected = new();
        private readonly ReactiveProperty<int> _score = new();
        private readonly GameArgsData _saveData;

        internal PlayerGameArgs(GameMode id)
        {
            Id = id;
            _sync = new SyncArraySource(
                _isSelected.ToUnmanagedParticleSource(),
                _score.ToUnmanagedParticleSource());
        }

        public GameMode Id { get; }

        public bool IsSelected { get => _isSelected.Value; internal set => _isSelected.Value = value; }

        public int Score { get => _score.Value; internal set => _score.Value = value; }

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        public void Load(GameArgsData data)
        {
            Score = data.Score;
        }

        public GameArgsData Save()
        {
            return new(
                (int)Id,
                Score);
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

        internal void Reset()
        {
            IsSelected = false;
            Score = 0;
        }
    }
}
