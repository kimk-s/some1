using System;
using System.Buffers;
using System.Diagnostics;
using System.Linq;
using MemoryPack;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;
using R3;

namespace Some1.Play.Core.Internal
{
    internal sealed class RankingGroup : ISyncWritable
    {
        private readonly SyncArraySource _sync;
        private readonly Func<PlayerGroup> _getPlayers;
        private readonly ITime _time;
        private readonly ReactiveProperty<Ranking>[] _all;
        private PlayerGroup? _players;
        private DateTime _updatedTimeUtc;

        internal RankingGroup(Func<PlayerGroup> getPlayers, ITime time)
        {
            _getPlayers = getPlayers;
            _time = time;
            _all = new ReactiveProperty<Ranking>[PlayConst.RankingCount];
            for (int i = 0; i < _all.Length; i++)
            {
                _all[i] = new();
            }

            _sync = new SyncArraySource(_all.Select(x => x.ToPackableParticleSource()));
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            writer.WriteSourceDirtySafely(_sync, mode);
        }

        internal void Update()
        {
            if (_updatedTimeUtc != DateTime.MinValue && _updatedTimeUtc.Minute == _time.UtcNow.Minute)
            {
                return;
            }
            _updatedTimeUtc = _time.UtcNow;

            _players ??= _getPlayers();

            var players = _players
                .GetUidPlayers()
                .Where(x => GetScore(x) > 0)
                .OrderByDescending(x => GetScore(x))
                .Take(_all.Length)
                .ToArray();
            byte number = 0;

            int count = Math.Max(_all.Length, players.Length);
            for (int i = 0; i < count; i++)
            {
                var item = i < _all.Length ? _all[i] : null;
                var player = i < players.Length ? players[i] : null;

                if (item is null)
                {
                    Debug.Assert(player is not null);
                    _players.SetMedal(player.Title.PlayerId.EnsureNotEmpty(), Medal.None);
                }
                else if (player is null)
                {
                    item.Value = Ranking.Empty;
                }
                else
                {
                    if (i == 0 || GetScore(players[i - 1]) != GetScore(players[i]))
                    {
                        number = checked((byte)(i + 1));
                    }

                    _players.SetMedal(player.Title.PlayerId.EnsureNotEmpty(), Ranking.GetMedal(number));

                    item.Value = new Ranking(
                        number,
                        GetScore(player),
                        player.Title.Title);
                }
            }
        }

        private static int GetScore(IPlayer player) => player.GameArgses.All[GameMode.Challenge].Score;
    }
}
