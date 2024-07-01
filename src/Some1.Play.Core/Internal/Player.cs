using System;
using System.Collections.Generic;
using System.Net.Sockets;
using MemoryPack;
using Microsoft.Extensions.Logging;
using R3;
using Some1.Net;
using Some1.Play.Core.Paralleling;
using Some1.Play.Data;
using Some1.Play.Info;
using Some1.Sync;
using Some1.Sync.Sources;

namespace Some1.Play.Core.Internal
{
    internal sealed class Player : IPlayer
    {
        private readonly string _playId;
        private readonly MemoryPackSerializerOptions _memoryPackSerializerOptions;
        private readonly SyncPipe _syncPipe;
        private readonly PlayRequest _request;
        private readonly PlayResponse _response;
        private readonly PlayerUnary _unary;
        private readonly PlayerGameManager _gameManager;
        private readonly PlayerGameArgsGroup _gameArgses;
        private readonly PlayerGame _game;
        private readonly PlayerGameResultGroup _gameResults;
        private readonly PlayerExp _exp;
        private readonly PlayerPremium _premium;
        private readonly PlayerCharacterGroup _characters;
        private readonly PlayerSpecialtyGroup _specialties;
        private readonly PlayerTitle _title;
        private readonly PlayerEmoji _emoji;
        private readonly PlayerWelcome _welcome;
        private readonly Leader _leader;
        private readonly ObjectGroup _objects;
        private readonly Object _object;
        private readonly RegionGroup _regions;
        private readonly ITime _time;
        private readonly ILogger<Player> _logger;
        private readonly PlayUserData _save = new() { PackedData = new() };
        private PipeState _pipeState;
        private DateTime _pipeStateTime;
        private bool _isPlayingToSave;

        internal Player(
            string playId,
            CharacterInfoGroup characterInfos,
            CharacterSkinInfoGroup characterSkinInfos,
            SpecialtyInfoGroup specialtyInfos,
            IObjectFactory objectFactory,
            RankingGroup rankings,
            RegionGroup regions,
            Space space,
            ITime time,
            ILogger<Player> logger)
        {
            _logger = logger;
            _time = time;
            _playId = playId;
            _regions = regions;
            _object = objectFactory.CreateRoot(true);
            _object.UpdateBefore += Object_UpdateBefore;
            _object.LikeAdded += Object_LikeAdded;
            _objects = new(_object, space);
            _leader = new(_object, space);
            _leader.LeadCompleted += Leader_LeadCompleted;
            _welcome = new();
            _emoji = new(_object, _time);
            _title = new(_object, characterInfos.WherePlayerById.Count, specialtyInfos.ById.Count);
            _specialties = new(specialtyInfos, _title, _time);
            _characters = new(characterInfos, characterSkinInfos, _title, _object, _time);
            _premium = new(_characters, _time);
            _gameResults = new();
            _exp = new(_time);
            _game = new(_object, _time);
            _gameArgses = new();
            _gameManager = new(
                _gameArgses,
                _game,
                _gameResults,
                _exp,
                _characters,
                _specialties,
                _object,
                _regions,
                _time);
            _unary = new(_gameManager, _characters, _title, _emoji, _welcome, _object);

            _request = new();
            _response = new(
                new SyncArraySource(
                    _unary,
                    _gameManager,
                    _gameArgses,
                    _game,
                    _gameResults,
                    _exp,
                    _premium,
                    _characters,
                    _specialties,
                    _title,
                    _emoji,
                    _welcome,
                    new PlayerObject(_object, _time)),
                rankings,
                _objects,
                time);
            _syncPipe = new(_request, _response);
            _memoryPackSerializerOptions = MemoryPackSerializerOptions.Default with { ServiceProvider = new ObjectIdServiceProvider(_object.Id) };

            _request.Unary.Subscribe(x => SyncRequestUnary_OnNext(x));
            _request.Cast.Subscribe(x => SyncRequestCast_OnNext(x));
            _request.Walk.Subscribe(x => SyncRequestWalk_OnNext(x));
        }

        public string? UserId { get; private set; }

        public PipeState PipeState
        {
            get => _pipeState;

            private set
            {
                _pipeState = value;
                _pipeStateTime = _time.UtcNow;

                _object.SetCast(null);
                _object.SetWalk(null);

                if (value.Exception is not null)
                {
                    _logger.LogWarning(value.Exception, "");
                }
            }
        }

        public PlayerDataStatus DataStatus { get; private set; }

        public bool Manager { get; private set; }

        public IPlayerUnary Unary => _unary;

        public IPlayerGameManager GameManager => _gameManager;

        public IPlayerGameArgsGroup GameArgses => _gameArgses;

        public IPlayerGame Game => _game;

        public IPlayerGameResultGroup GameResults => _gameResults;

        public IPlayerExp Exp => _exp;

        public IPlayerPremium Premium => _premium;

        public IPlayerCharacterGroup Characters => _characters;

        public IReadOnlyDictionary<SpecialtyId, IPlayerSpecialty> Specialties => _specialties.All;

        public IPlayerTitle Title => _title;

        public IPlayerEmoji Emoji => _emoji;

        public IPlayerWelcome Welcome => _welcome;

        public ILeader Leader => _leader;

        public IObject Object => _object;

        public bool IsPlayingToSave
        {
            get => _isPlayingToSave;

            set
            {
                if (_isPlayingToSave == value)
                {
                    return;
                }

                _isPlayingToSave = value;
                _gameManager.IsPlayingToSave = value;
            }
        }

        internal void SetUidPipe(UidPipe uidPipe)
        {
            if (uidPipe.Uid is null || uidPipe.Pipe is null)
            {
                throw new ArgumentOutOfRangeException(nameof(uidPipe));
            }

            if (UserId is not null && UserId != uidPipe.Uid)
            {
                throw new InvalidOperationException();
            }

            UserId = uidPipe.Uid;
            _syncPipe.SetPipe(uidPipe.Pipe);
            PipeState = new(PipeStatus.Processing);
        }

        internal void SetMedal(Medal medal)
        {
            _title.SetMedal(medal);
        }

        internal string BeginLoad()
        {
            if (UserId is null || (DataStatus != PlayerDataStatus.None && DataStatus != PlayerDataStatus.BeginLoad))
            {
                throw new InvalidOperationException();
            }

            DataStatus = PlayerDataStatus.BeginLoad;

            return UserId;
        }

        internal void EndLoad(PlayUserData data)
        {
            if (UserId is null || DataStatus != PlayerDataStatus.BeginLoad)
            {
                throw new InvalidOperationException();
            }

            if (data.Id != UserId || data.PlayId != _playId || !data.IsPlaying)
            {
                throw new ArgumentException("", nameof(data));
            }

            DataStatus = PlayerDataStatus.EndLoad;

            UserId = data.Id;
            Manager = data.Manager;
            _gameArgses.Load(data.PackedData?.GameArgses);
            _gameResults.Load(data.PackedData?.GameResults);
            _exp.Load(data.PackedData?.Exp);
            _premium.Set(data.Premium);
            _characters.Load(data.PackedData?.Characters);
            _specialties.Load(data.PackedData?.Specialties);
            _title.Load(data.PackedData?.Title);
            _emoji.Load(data.PackedData?.Emoji);
            _welcome.Load(data.PackedData?.Welcome);

            _leader.Enable();
            _object.SetBattle(false);
            _object.SetTeamProperty(Team.Player);
            _object.WarpTransfer(_regions.GetTownWarpPosition(null), null);
            _object.SetTransformRotation(-90);
        }

        internal void SetPlayerId(PlayerId value)
        {
            value.EnsureNotEmpty();

            _title.SetPlayerId(value);
        }

        internal PlayUserData BeginSave()
        {
            if (UserId is null || (DataStatus != PlayerDataStatus.EndLoad && DataStatus != PlayerDataStatus.BeginSave && DataStatus != PlayerDataStatus.EndSave))
            {
                throw new InvalidOperationException();
            }

            DataStatus = PlayerDataStatus.BeginSave;

            IsPlayingToSave = PipeState.Status switch
            {
                PipeStatus.None => false,
                PipeStatus.Processing => true,
                PipeStatus.Finishing => false,
                PipeStatus.Finished => false,
                PipeStatus.Faulted => (_time.UtcNow - _pipeStateTime).TotalSeconds < PlayConst.PlayerFaultedTime,
                _ => throw new InvalidOperationException(),
            };

            var save = _save;

            save.Id = UserId;
            save.PlayId = _playId;
            save.IsPlaying = IsPlayingToSave;
            save.PackedData!.GameArgses = _gameArgses.Save();
            save.PackedData.GameResults = _gameResults.Save();
            save.PackedData.Exp = _exp.Save();
            save.PackedData.Characters = _characters.Save();
            save.PackedData.Specialties = _specialties.Save();
            save.PackedData.Title = _title.Save();
            save.PackedData.Emoji = _emoji.Save();
            save.PackedData.Welcome = _welcome.Save();

            return save;
        }

        internal void EndSave(PlaySaveUserResult data)
        {
            if (UserId is null || DataStatus != PlayerDataStatus.BeginSave)
            {
                throw new InvalidOperationException();
            }

            if (data.Id != UserId)
            {
                throw new ArgumentException("", nameof(data));
            }

            _premium.Set(data.Premium);

            DataStatus = PlayerDataStatus.EndSave;

            if (!IsPlayingToSave)
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Finished);
            }
        }

        internal bool IsCompleted()
        {
            if (UserId is null)
            {
                throw new InvalidOperationException();
            }

            if (IsPlayingToSave || !DataStatus.IsSaved())
            {
                return false;
            }

            return true;
        }

        internal void Reset()
        {
            UserId = null;
            _syncPipe.Complete();
            PipeState = new(PipeStatus.None);
            DataStatus = PlayerDataStatus.None;
            IsPlayingToSave = false;
            Manager = false;
            _request.Reset();
            _unary.Reset();
            _gameManager.Reset();
            _gameArgses.Reset();
            _game.Reset();
            _gameResults.Reset();
            _exp.Reset();
            _premium.Reset();
            _characters.Reset();
            _specialties.Reset();
            _title.Reset();
            _emoji.Reset();
            _welcome.Reset();
            _leader.Disable();
            _object.Reset(null);
        }

        private void Object_UpdateBefore(object? _, ParallelToken e)
        {
            _response.ClearDirty();

            ReadPipe();

            _gameManager.Update(_time.DeltaSeconds, e);
            _game.Update(_time.DeltaSeconds);
            _exp.Update();
            _premium.Update();
            _characters.Update();
            _emoji.Update(_time.DeltaSeconds);
        }

        private void Object_LikeAdded(object? _, Like e)
        {
            _title.AddLikePoint(e);
        }

        private void Leader_LeadCompleted(object? _, (LeaderStatus, ParallelToken) e)
        {
            if (e.Item1 == LeaderStatus.Lead3Completed)
            {
                WritePipe(e.Item2);
            }
        }

        private void Exit()
        {
            if (Object.Region.Section?.Type.IsBattle() == true)
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Faulted);
            }
            else
            {
                FinishPipe();
            }
        }

        private void FinishPipe()
        {
            if (_syncPipe.IsInputCompleted)
            {
                return;
            }

            _syncPipe.CompleteInput();
            PipeState = new(PipeStatus.Finishing);
        }

        private void ReadPipe()
        {
            if (_syncPipe.IsInputCompleted)
            {
                return;
            }

            SyncPipeReadResult result;
            try
            {
                result = _syncPipe.Read(10);
            }
            catch (Exception ex)
            when (ex is OperationCanceledException
                || (ex is SocketException socketException && socketException.SocketErrorCode.IsKnown()))
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Faulted);
                return;
            }
            catch (Exception ex)
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Faulted, ex);
                return;
            }

            if (result.IsCompleted)
            {
                Exit();
            }
        }

        private void WritePipe(ParallelToken parallelToken)
        {
            if (_syncPipe.IsOutputCompleted)
            {
                return;
            }

            _objects.PrepareWrite(parallelToken);

            SyncPipeWriteResult result;
            try
            {
                result = _syncPipe.Write(_memoryPackSerializerOptions);
            }
            catch (Exception ex)
            when (ex is OperationCanceledException
                || (ex is SocketException socketException && socketException.SocketErrorCode.IsKnown()))
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Faulted);
                return;
            }
            catch (Exception ex)
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Faulted, ex);
                return;
            }

            if (result.IsCompleted)
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Faulted);
            }
            else if (result.IsPending)
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Faulted);
            }
        }

        private void SyncRequestUnary_OnNext(Unary? unary)
        {
            if (!DataStatus.IsLoaded())
            {
                return;
            }

            _unary.Set(unary);
        }

        private void SyncRequestCast_OnNext(Cast? cast)
        {
            if (!DataStatus.IsLoaded())
            {
                return;
            }

            _object.SetCast(cast);
        }

        private void SyncRequestWalk_OnNext(Walk? walk)
        {
            if (!DataStatus.IsLoaded())
            {
                return;
            }

            _object.SetWalk(walk);
        }
    }

    internal sealed class ObjectIdServiceProvider : IServiceProvider
    {
        public ObjectIdServiceProvider(int objectId) => ObjectId = objectId;

        public int ObjectId { get; }

        public object? GetService(Type serviceType) => throw new NotSupportedException();
    }
}
