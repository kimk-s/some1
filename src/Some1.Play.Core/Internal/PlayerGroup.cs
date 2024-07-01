using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Some1.Play.Core.Internal.Channels;
using Some1.Play.Core.Options;
using Some1.Play.Data;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerGroup : IPlayerGroup, IDisposable
    {
        private readonly CancellationTokenSource _cts = new();
        private readonly UidPipeChannel _channel;
        private readonly IPlayRepository _repository;
        private readonly ITime _time;
        private readonly string _playId;
        private readonly Queue<Player> _nonUids;
        private readonly Dictionary<string, Player> _byUid = new();
        private readonly Dictionary<PlayerId, Player> _byPlayerId = new();
        private readonly PlayerIdManager _playerIdManager;
        private readonly Player[] _all;
        private readonly ILogger<PlayerGroup> _logger;
        private readonly PlayBusy _busy;

        private readonly WorkStopwatch _workStopwatch = new();

        private string[]? _loadInput;
        private Task<PlayUserData[]>? _loadOutputTask;
        private Memory<PlayUserData>? _loadOutputWork;
        private readonly float _loadThrottle;
        private DateTime _loadTime;

        private PlayUserData[]? _saveInput;
        private Task<PlaySaveUserResult[]>? _saveOutputTask;
        private Memory<PlaySaveUserResult>? _saveOutputWork;
        private readonly float _saveThrottle;
        private DateTime _saveTime;

        private Memory<Player>? _resetWork;

        private int _statisticsTotalSet;
        private int _statisticsTotalReset;
        private int _statisticsTotalLoad;
        private int _statisticsLogFrame;

        internal PlayerGroup(
            string playId,
            PlayerGroupOptions options,
            IPlayRepository repository,
            CharacterInfoGroup characterInfos,
            CharacterSkinInfoGroup characterSkinInfos,
            SpecialtyInfoGroup specialtyInfos,
            IObjectFactory objectFactory,
            RankingGroup rankings,
            RegionGroup regions,
            Space space,
            ITime time,
            ILoggerFactory loggerFactory)
        {
            _channel = new();
            _repository = repository;
            _time = time;
            _playId = playId;
            _all = Enumerable.Range(0, options.Count)
                .Select(_ => new Player(
                    playId,
                    characterInfos,
                    characterSkinInfos,
                    specialtyInfos,
                    objectFactory,
                    rankings,
                    regions,
                    space,
                    time,
                    loggerFactory.CreateLogger<Player>()))
                .ToArray();
            _nonUids = new(_all);
            _playerIdManager = new(time);
            _logger = loggerFactory.CreateLogger<PlayerGroup>();
            _busy = new(_all.Length, options.Busy);
            _loadThrottle = options.LoadThrottle;
            _saveThrottle = options.SaveThrottle;
        }

        public IReadOnlyList<IPlayer> All => _all;

        public int UidCount => _byUid.Count;

        public int NonUidCount => _nonUids.Count;

        internal PlayBusy Busy => _busy;

        internal bool Maintenance { get; private set; }

        public IEnumerable<IPlayer> GetUidPlayers()
        {
            return _byUid.Values;
        }

        public void Dispose()
        {
            _cts.Cancel();
        }

        internal ValueTask AddUidPipeAsync(UidPipe uidPipe, CancellationToken cancellationToken)
        {
            return _channel.Writer.WriteAsync(uidPipe, cancellationToken);
        }

        internal void Update1()
        {
            Set();
            Load();
        }

        internal void Update2()
        {
            Save();
            ResetIfCompleted();
            _playerIdManager.Update();
            _busy.Update(_byUid.Count);

            //if (_statisticsLogFrame++ > 600)
            //{
            //    _statisticsLogFrame = 0;
            //    _logger.LogInformation($"Statistics] Uids:{_byUid.Count}, TotalSet:{_statisticsTotalSet}, TotalLoad:{_statisticsTotalLoad}, TotalReset:{_statisticsTotalReset}");
            //}
        }

        internal void SetMedal(PlayerId playerId, Medal medal)
        {
            _byPlayerId[playerId].SetMedal(medal);
        }

        internal void SetMaintenance(bool maintenance)
        {
            if (Maintenance == maintenance)
            {
                return;
            }

            Maintenance = maintenance;

            if (maintenance)
            {
                ResetIfNotManager();
            }
        }

        private void Set()
        {
            const float WorkTimeRate = 0.1f;
            _workStopwatch.Start(_time.DeltaSeconds * WorkTimeRate);
            
            while (!_workStopwatch.TryStop() && _channel.Reader.TryRead(out var uidPipe))
            {
                if (!_byUid.TryGetValue(uidPipe.Uid, out var player))
                {
                    if (_busy.IsFull || !_nonUids.TryDequeue(out player))
                    {
                        uidPipe.Pipe.CompleteAndReset();

                        continue;
                    }

                    _byUid.Add(uidPipe.Uid, player);
                }

                player.SetUidPipe(uidPipe);

                _statisticsTotalSet++;
            }
        }

        private void Load()
        {
            if (_loadOutputTask is null)
            {
                if ((DateTime.UtcNow - _loadTime).TotalSeconds < _loadThrottle)
                {
                    return;
                }

                _loadTime = DateTime.UtcNow;

                var uids = BeginLoad();
                if (uids.Length == 0)
                {
                    return;
                }

                _loadInput = uids;
                _loadOutputTask = _repository.LoadUserAsync(_loadInput, _playId, _cts.Token);
            }

            if (_loadOutputTask.IsCompleted)
            {
                if (_loadOutputTask.IsCompletedSuccessfully)
                {
                    if (EndLoad(_loadOutputTask.Result))
                    {
                        _loadInput = null;
                        _loadOutputTask = null;
                    }
                }
                else
                {
                    _logger.LogError(_loadOutputTask.Exception, "Failed to load players.");
                    _loadInput = null;
                    _loadOutputTask = null;
                }
            }
        }

        private void Save()
        {
            if (_saveOutputTask is null)
            {
                if ((DateTime.UtcNow - _saveTime).TotalSeconds < _saveThrottle)
                {
                    return;
                }

                _saveTime = DateTime.UtcNow;

                var players = BeginSave();
                if (players.Length == 0)
                {
                    return;
                }

                _saveInput = players;
                _saveOutputTask = _repository.SaveUserAsync(_saveInput, _cts.Token);
            }

            if (_saveOutputTask.IsCompleted)
            {
                if (_saveOutputTask.IsCompletedSuccessfully)
                {
                    if (EndSave(_saveOutputTask.Result))
                    {
                        _saveInput = null;
                        _saveOutputTask = null;
                    }
                }
                else
                {
                    _logger.LogError(_saveOutputTask.Exception, "Failed to save players.");
                    _saveInput = null;
                    _saveOutputTask = null;
                }
            }
        }

        private string[] BeginLoad()
        {
            return _byUid.Values
                .Where(x => !x.DataStatus.IsLoaded())
                .Select(x => x.BeginLoad())
                .ToArray();
        }

        private bool EndLoad(PlayUserData[] users)
        {
            const float WorkTimeRate = 0.1f;
            _workStopwatch.Start(_time.DeltaSeconds * WorkTimeRate);
            int i = 0;
            _loadOutputWork ??= users.AsMemory();
            var span = _loadOutputWork.Value.Span;

            while (!_workStopwatch.TryStop() && i < span.Length)
            {
                var item = span[i];

                var player = _byUid[item.Id];

                bool maintenance = Maintenance && !item.Manager;
                bool pass = item.PlayId == _playId && item.IsPlaying;

                if (maintenance || !pass)
                {
                    Reset(player);
                }
                else
                {
                    player.EndLoad(item);
                    player.SetPlayerId(GetPlayerId(player.UserId!));
                    _byPlayerId.Add(player.Title.PlayerId.EnsureNotEmpty(), player);
                }

                i++;

                _statisticsTotalLoad++;
            }

            _loadOutputWork = i < span.Length ? _loadOutputWork.Value.Slice(i) : (Memory<PlayUserData>?)null;
            return _loadOutputWork is null;
        }

        private PlayUserData[] BeginSave()
        {
            var loadedPlayers = _byUid.Values
                .Where(x => x.DataStatus.IsLoaded())
                .ToArray();

            if (loadedPlayers.Length == 0)
            {
                return Array.Empty<PlayUserData>();
            }

            var result = new PlayUserData[loadedPlayers.Length];

            Parallel.ForEach(
                Partitioner.Create(0, loadedPlayers.Length),
                new ParallelOptions()
                {
                    CancellationToken = _cts.Token
                },
                source =>
                {
                    for (int i = source.Item1; i < source.Item2; i++)
                    {
                        result[i] = loadedPlayers[i].BeginSave();
                    }
                });

            return result;
        }

        private bool EndSave(PlaySaveUserResult[] results)
        {
            const float WorkTimeRate = 0.1f;
            _workStopwatch.Start(_time.DeltaSeconds * WorkTimeRate);
            int i = 0;
            _saveOutputWork ??= results.AsMemory();
            var span = _saveOutputWork.Value.Span;

            while (!_workStopwatch.TryStop() && i < span.Length)
            {
                var item = span[i];

                var player = _byUid[item.Id];
                player.EndSave(item);

                i++;
            }

            _saveOutputWork = i < span.Length ? _saveOutputWork.Value.Slice(i) : (Memory<PlaySaveUserResult>?)null;
            return _saveOutputWork is null;
        }

        private void ResetIfNotManager()
        {
            foreach (var item in _byUid.Values.ToArray())
            {
                if (!item.Manager)
                {
                    Reset(item);
                }
            }
        }

        private void ResetIfCompleted()
        {
            const float WorkTimeRate = 0.1f;
            _workStopwatch.Start(_time.DeltaSeconds * WorkTimeRate);
            int i = 0;
            _resetWork ??= _all.AsMemory();
            var span = _resetWork.Value.Span;

            while (!_workStopwatch.TryStop() && i < span.Length)
            {
                var item = span[i];

                if (item.UserId is not null && item.IsCompleted())
                {
                    Reset(item);
                }

                i++;
            }

            _resetWork = i < span.Length ? _resetWork.Value.Slice(i) : (Memory<Player>?)null;
        }

        private void Reset(Player player)
        {
            string userId = player.UserId ?? throw new InvalidOperationException();
            PlayerId playerId = player.Title.PlayerId;

            player.Reset();

            _byUid.Remove(userId);
            _nonUids.Enqueue(player);

            if (playerId != PlayerId.Empty)
            {
                _byPlayerId.Remove(playerId);
                _playerIdManager.Add(userId, playerId);
            }

            _statisticsTotalReset++;
        }

        private PlayerId GetPlayerId(string userId)
        {
            if (!_playerIdManager.Remove(userId, out var playerId))
            {
                playerId = NewPlayerId();
            }
            return playerId;
        }

        private PlayerId NewPlayerId()
        {
            int count = 0;
            while (true)
            {
                var playerId = PlayerId.NewPlayerId().EnsureNotEmpty();
                if (!_byPlayerId.ContainsKey(playerId) && !_playerIdManager.Contains(playerId))
                {
                    return playerId;
                }

                count++;
                if (count == 1000)
                {
                    _logger.LogWarning($"New player id count is over {count}.");
                }

                if (count > 10000)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        class WorkStopwatch
        {
            private const byte CheckStopInterval = 0xf;
            private readonly Stopwatch _stopwatch = new();
            private double _runningTime;
            private byte _stopCount;

            public void Start(double runningSeconds)
            {
                _runningTime = runningSeconds;
                _stopCount = 0;
                _stopwatch.Restart();
            }

            public bool TryStop()
            {
                if (_stopCount++ < CheckStopInterval)
                {
                    return false;
                }

                if (_stopwatch.Elapsed.TotalSeconds < _runningTime)
                {
                    return false;
                }

                _stopwatch.Stop();
                return true;
            }
        }
    }
}
