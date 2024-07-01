using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Some1.Play.Core.Internal;
using Some1.Play.Core.Options;
using Some1.Play.Core.Paralleling;
using Some1.Play.Data;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public sealed class PlayCore : IPlayCore, IDisposable
    {
        private readonly ILogger<PlayCore> _logger;
        private readonly PlayPlay _play;
        private readonly LeaderGroup _leaders;
        private readonly PlayerGroup _players;
        private readonly NonPlayerGroup _nonPlayers;
        private readonly RankingGroup _rankings;
        private readonly RegionGroup _regions;
        private readonly Space _space;
        private readonly Time _time;

        public PlayCore(
            IClock clock,
            IPlayInfoRepository infoRepository,
            IPlayRepository playRepository,
            IOptions<PlayOptions> options,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PlayCore>();
            _time = new(clock);

            var info = infoRepository.Value;
            var opt = options.Value;

            _logger.LogInformation($"Parallel count: {opt.Parallel.Count}({opt.Parallel.GetSafeCount()}), Players count: {opt.Players.Count}");

            Id = opt.Id ?? throw new InvalidOperationException("Play Id is null. Please configure play id.");

            _space = new(info.Space, opt.Parallel, Time);

            _regions = new(info.Regions, info.RegionSections, info.Space);

            _rankings = new(() => _players ?? throw new InvalidOperationException(), _time);

            var objectFactory = CreateObjectFactory(info, opt);

            _nonPlayers = new(
                info.NonPlayerGenerations,
                info.NonPlayerSpawnings,
                info.Seasons,
                objectFactory,
                _regions,
                _space,
                _time);

            _players = new(
                Id,
                opt.Players,
                playRepository,
                info.Characters,
                info.CharacterSkins,
                info.Specialties,
                objectFactory,
                _rankings,
                _regions,
                _space,
                _time,
                loggerFactory);

            _leaders = new(_players.All.Select(x => (Leader)x.Leader), opt.Parallel);

            _play = new(loggerFactory.CreateLogger<PlayPlay>(), Id, _players.Busy, playRepository, _players);
        }

        public string Id { get; }

        public IReadOnlyList<ILeader> Leaders => _leaders.All;

        public IPlayerGroup Players => _players;

        public IEnumerable<INonPlayer> NonPlayers => _nonPlayers.All;

        public IRegionGroup Regions => _regions;

        public ISpace Space => _space;

        public ITime Time => _time;

        public ValueTask AddUidPipeAsync(UidPipe uidPipe, CancellationToken cancellationToken)
        {
            return _players.AddUidPipeAsync(uidPipe, cancellationToken);
        }

        private readonly Statistics _statistics = new("Frame Time");
        private int _statisticsFrameCount;

        private class Statistics
        {
            private readonly Stopwatch _stopwatch = new();
            private readonly string _name;
            private double _totalElapsed;
            private double _highElapsed;
            private int _count;

            public Statistics(string name)
            {
                _name = name;
            }

            private double Average => _count < 1 ? _totalElapsed : _totalElapsed / (_count - 1);

            public void Execute(Action action)
            {
                _stopwatch.Restart();

                action();

                _stopwatch.Stop();

                if (_stopwatch.Elapsed.TotalSeconds > _highElapsed)
                {
                    _highElapsed = _stopwatch.Elapsed.TotalSeconds;
                }

                _totalElapsed += _stopwatch.Elapsed.TotalSeconds;

                _count++;
            }

            public string GetString()
            {
                return $"{_name} Elapsed] Average:{Average:N3}s, Max:{_highElapsed:N3}s";
            }

            public void Reset()
            {
                _totalElapsed = 0;
                _highElapsed = 0;
                _count = 0;
            }
        }

        public void Update(float deltaSeconds)
        {
            _statistics.Execute(() =>
            {
                _time.Update(deltaSeconds);

                _play.Update1();
                _players.Update1();
                _leaders.Lead1();
                _players.Update2();
                _leaders.Lead2();
                _rankings.Update();
                _leaders.Lead3();
                _play.Update2();
            });

            if (_statisticsFrameCount++ > 600)
            {
                _logger.LogInformation(_statistics.GetString());

                _statisticsFrameCount = 0;
                _statistics.Reset();
            }
        }

        public void Dispose()
        {
            _play.Dispose();
            _players.Dispose();
        }

        private ObjectFactory CreateObjectFactory(PlayInfo info, PlayOptions options)
        {
            return new(
                info.Characters,
                info.CharacterAlives,
                info.CharacterIdles,
                info.CharacterCasts,
                info.CharacterCastStats,
                info.CharacterStats,
                info.CharacterEnergies,
                info.CharacterSkins,
                info.CharacterSkinEmojis,
                info.Buffs,
                info.BuffStats,
                info.Boosters,
                info.Specialties,
                info.Triggers,
                options.Parallel,
                _regions,
                _space,
                _time);
        }
    }
}
