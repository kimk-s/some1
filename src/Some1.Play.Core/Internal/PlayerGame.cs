using System.Buffers;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerGame : IPlayerGame, ISyncSource
    {
        private readonly SyncArraySource _sync;
        private readonly ReactiveProperty<Game?> _game = new();
        private readonly ReactiveProperty<FloatWave> _cycles = new();

        public PlayerGame(IObject @object, ITime time)
        {
            _sync = new SyncArraySource(
                _game.ToNullableUnmanagedParticleSource(),
                _cycles.ToWaveSource(time));

            @object.TakeStuffs.Added += Stuffs_Added;
        }

        public Game? Game { get => _game.Value; private set => _game.Value = value; }

        public float Cycles => _cycles.Value.B;

        public ReadOnlyReactiveProperty<bool> Dirty => _sync.Dirty;

        public ReadOnlyReactiveProperty<bool> IsDefault => _sync.IsDefault;

        internal void Set(GameMode game)
        {
            Game = new(game, 0);
            _cycles.Value = default;
        }

        internal void Update(float deltaSeconds)
        {
            if (Game is null)
            {
                return;
            }

            _cycles.Value = _cycles.Value.Flow(deltaSeconds);
        }

        internal void Reset()
        {
            Game = null;
            _cycles.Value = default;
        }

        private void Stuffs_Added(object? sender, Stuff e)
        {
            AddScore(e.Score);
        }

        private void AddScore(int value)
        {
            if (Game is not null)
            {
                Game = new Game(Game.Value.Mode, Game.Value.Score + value);
            }
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
