using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using R3;
using Some1.Net;
using Some1.Play.Client;
using Some1.Play.Front.Internal;
using Some1.Play.Info;
using Some1.Sync;

namespace Some1.Play.Front
{
    public sealed class PlayFront : IPlayFront, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly IPlayClient _client;
        private readonly IPlayStreamer _streamer;
        private readonly ILogger<PlayFront> _logger;
        private readonly ReactiveProperty<PipeState> _pipeState = new();
        private readonly SyncPipe _syncPipe;
        private readonly PlayRequestFront _request;
        private readonly PlayResponseFront _response;
        private readonly ReactiveProperty<Unary?> _unaryRequest = new();
        private readonly ReactiveProperty<Cast?> _castRequest = new();
        private readonly ReactiveProperty<Walk?> _walkRequest = new();
        private readonly ReactiveProperty<int> _attackFailNotAnyLoadCount = new();
        private readonly RegionGroupFront _regions;
        private readonly FloorGroupFront _floors;
        private readonly SpaceFront _space;
        private readonly ReactiveProperty<float> _localTimeScale = new();
        private TaskCompletionSource<bool>? _unaryTcs;
        private int _unaryToken;

        public PlayFront(
            IPlayInfoRepository infoRepository,
            IPlayClient client,
            IPlayStreamer streaming,
            ILogger<PlayFront> logger)
        {
            _client = client;
            _streamer = streaming;
            _logger = logger;

            var info = infoRepository.Value;

            _pipeState.AddTo(_disposables);
            _space = new SpaceFront(info.Space);
            _regions = new RegionGroupFront(info.Regions, info.RegionSections, info.Space);

            _request = new PlayRequestFront(
                _unaryRequest,
                _castRequest,
                _walkRequest).AddTo(_disposables);
            _response = new PlayResponseFront(
                info,
                _regions).AddTo(_disposables);
            _syncPipe = new SyncPipe(_response, _request);

            IsUnaryRunning = UnaryRequest
                .CombineLatest(Player.Unary.Result, (req, res) => req != res?.Unary)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            _attackFailNotAnyLoadCount.AddTo(_disposables);

            _floors = new FloorGroupFront(Regions, Player, Space, info.Space).AddTo(_disposables);

            _localTimeScale.AddTo(_disposables);

            _streamer.Setup(
                () => Time.TotalSecondsWave.CurrentValue,
                ReadPipe,
                GetPipeReadableDeltaTime,
                _response.Interpolate,
                x => _localTimeScale.Value = x);
        }

        ReadOnlyReactiveProperty<PipeState> IPlayFront.PipeState => _pipeState;

        public ReadOnlyReactiveProperty<Unary?> UnaryRequest => _unaryRequest;

        public ReadOnlyReactiveProperty<bool> IsUnaryRunning { get; }

        public ReadOnlyReactiveProperty<Cast?> CastRequest => _castRequest;

        public ReadOnlyReactiveProperty<Walk?> MoveRequest => _walkRequest;

        public ReadOnlyReactiveProperty<int> AttackFailNotAnyLoadCount => _attackFailNotAnyLoadCount;

        public IPlayerFront Player => _response.Player;

        public IRankingGroupFront Rankings => _response.Rankings;

        public IObjectGroupFront Objects => _response.Objects;

        public IReadOnlyList<IFloorFront> Floors => _floors.All;

        public IRegionGroupFront Regions => _regions;

        public ISpaceFront Space => _space;

        public ITimeFront Time => _response.Time;

        public ReadOnlyReactiveProperty<float> LocalTimeScale => _localTimeScale;

        public PipeState PipeState
        {
            get => _pipeState.Value;

            private set
            {
                try
                {
                    _pipeState.Value = value;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, $"Exception thrown on set pipe state '{value}'.");
                }
            }
        }

        public void Dispose()
        {
            _syncPipe.Complete();
            PipeState = new(PipeStatus.None);
            _disposables.Dispose();
        }

        public async Task StartPipeAsync(CancellationToken cancellationToken)
        {
            if (PipeState.IsRunning)
            {
                throw new InvalidOperationException();
            }

            _castRequest.Value = null;
            _walkRequest.Value = null;
            _syncPipe.Complete();
            PipeState = new(PipeStatus.Starting);

            DuplexPipe pipe;
            try
            {
                pipe = await _client.StartPipeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                PipeState = new(PipeStatus.Faulted, ex);
                _logger.LogInformation($"Exception thrown on {nameof(StartPipeAsync)}. {ex.ToShortString()}");
                return;
            }

            _syncPipe.SetPipe(pipe);
            PipeState = new(PipeStatus.Processing);
            _streamer.ResetState();
        }

        public PlayFrontError GetExitError()
        {
            if (Player.Object.Region.Section.CurrentValue?.Type.IsBattle() == true)
            {
                return PlayFrontError.Field;
            }

            return PlayFrontError.Success;
        }

        public PlayFrontError Exit()
        {
            var error = GetExitError();
            if (error != PlayFrontError.Success)
            {
                return error;
            }

            FinishPipe();
            return PlayFrontError.Success;
        }

        public void FinishPipe()
        {
            if (_syncPipe.IsPipeNull || _syncPipe.IsOutputCompleted)
            {
                return;
            }

            _syncPipe.CompleteOutput();
            PipeState = new(PipeStatus.Finishing);
        }

        public void UpdatePipe(float deltaSeconds)
        {
            if (deltaSeconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (_syncPipe.IsPipeNull)
            {
                return;
            }

            WritePipe();
            _streamer.Update(deltaSeconds);

            CompleteUnaryTcs();
        }

        private bool ReadPipe()
        {
            if (_syncPipe.IsPipeNull || _syncPipe.IsInputCompleted)
            {
                return false;
            }

            SyncPipeReadResult result;
            try
            {
                result = _syncPipe.Read(1);
            }
            catch (Exception ex)
            {
                _syncPipe.Complete();
                PipeState = new(PipeStatus.Faulted, ex);
                _logger.LogInformation($"Exception thrown on {nameof(ReadPipe)}. {ex.ToShortString()}");
                return false;
            }

            if (result.IsCompleted)
            {
                _syncPipe.Complete();
                PipeState = _pipeState.Value.Status == PipeStatus.Finishing
                    ? new(PipeStatus.Finished)
                    : new(PipeStatus.Terminated);
            }

            return result.BodyReadCount > 0;
        }

        private float GetPipeReadableDeltaTime(float maxDeltaTime)
        {
            try
            {
                return _syncPipe.GetReadableDeltaTime(maxDeltaTime);
            }
            catch
            {
                return 0;
            }
        }

        private void WritePipe()
        {
            if (_syncPipe.IsPipeNull || _syncPipe.IsOutputCompleted)
            {
                return;
            }

            SyncPipeWriteResult result;
            try
            {
                result = _syncPipe.Write();
            }
            catch
            {
                _syncPipe.CompleteOutput();
                return;
            }
            finally
            {
                _request.ClearDirty();
            }

            if (result.IsCompleted)
            {
                _syncPipe.CompleteOutput();
            }
            else if (result.IsPending)
            {
                _logger.LogInformation($"SyncPipe.Write is pending.");
                _syncPipe.CompleteOutput();
            }
        }

        public void Cast(Cast cast)
        {
            var item = _response.Player.Object.Cast.Items[cast.Id];
            if (!item.AnyLoadCount.CurrentValue)
            {
                if (item.Id == CastId.Attack)
                {
                    _attackFailNotAnyLoadCount.Value++;
                }
                return;
            }

            _castRequest.Value = cast;
        }

        public void Walk(Walk move)
        {
            _walkRequest.Value = move;
        }

        public Task ClearUnaryAsync(CancellationToken cancellationToken)
        {
            return UnaryAsync(null, cancellationToken);
        }

        public async Task<PlayFrontError> SelectCharacterAsync(CharacterId id, CancellationToken cancellationToken)
        {
            if (Player.Characters.All[id].IsSelected.CurrentValue)
            {
                return PlayFrontError.Success;
            }

            if (!Player.Characters.All[id].IsUnlocked.CurrentValue)
            {
                return PlayFrontError.Locked;
            }

            if (Player.Object.Region.Section.CurrentValue?.Type.IsBattle() == true)
            {
                return PlayFrontError.Field;
            }

            await UnaryAsync(UnaryType.SelectCharacter, (int)id, default, default, cancellationToken);

            return PlayFrontError.Success;
        }

        public async Task<PlayFrontError> SetCharacterIsRandomSkinAsync(CharacterId id, bool value, CancellationToken cancellationToken)
        {
            if (Player.Characters.All[id].IsRandomSkin.CurrentValue == value)
            {
                return PlayFrontError.Success;
            }

            if (Player.Object.Region.Section.CurrentValue?.Type.IsBattle() == true)
            {
                return PlayFrontError.Field;
            }

            await UnaryAsync(UnaryType.SetCharacterIsRandomSkin, (int)id, value ? 1 : 0, default, cancellationToken);
            return PlayFrontError.Success;
        }

        public async Task<PlayFrontError> SelectCharacterSkinAsync(CharacterId id, SkinId skinId, CancellationToken cancellationToken)
        {
            if (Player.Characters.All[id].Skins[skinId].IsSelected.CurrentValue)
            {
                return PlayFrontError.Success;
            }

            if (!Player.Characters.All[id].Skins[skinId].IsUnlocked.CurrentValue)
            {
                return PlayFrontError.Locked;
            }

            if (Player.Object.Region.Section.CurrentValue?.Type.IsBattle() == true)
            {
                return PlayFrontError.Field;
            }

            await UnaryAsync(UnaryType.SelectCharacterSkin, (int)id, (int)skinId, default, cancellationToken);
            return PlayFrontError.Success;
        }

        public async Task<PlayFrontError> SetCharacterSkinIsRandomSelectedAsync(CharacterId id, SkinId skinId, bool value, CancellationToken cancellationToken)
        {
            if (Player.Characters.All[id].Skins[skinId].IsRandomSelected.CurrentValue == value)
            {
                return PlayFrontError.Success;
            }

            if (!Player.Characters.All[id].Skins[skinId].IsUnlocked.CurrentValue)
            {
                return PlayFrontError.Locked;
            }

            if (Player.Object.Region.Section.CurrentValue?.Type.IsBattle() == true)
            {
                return PlayFrontError.Field;
            }

            await UnaryAsync(UnaryType.SetCharacterSkinIsRandomSelected, (int)id, (int)skinId, value ? 1 : 0, cancellationToken);
            return PlayFrontError.Success;
        }

        public Task SetEmojiAsync(EmojiId id, CancellationToken cancellationToken)
        {
            if (Player.Emojis.Delay.CurrentValue > 0)
            {
                return Task.CompletedTask;
            }

            return UnaryAsync(UnaryType.SetEmoji, (int)id, default, default, cancellationToken);
        }

        public Task SelelctGameArgsAsync(GameMode id, CancellationToken cancellationToken)
        {
            if (Player.GameArgses.Selected.CurrentValue?.Id == id)
            {
                return Task.CompletedTask;
            }

            return UnaryAsync(UnaryType.SetGameArgs, (int)id, default, default, cancellationToken);
        }

        public Task ReadyGameAsync(CancellationToken cancellationToken)
        {
            return UnaryAsync(UnaryType.ReadyGame, default, default, default, cancellationToken);
        }

        public async Task<PlayFrontError> ReturnGameAsync(CancellationToken cancellationToken)
        {
            if (Player.GameManager.Status.CurrentValue == GameManagerStatus.Return)
            {
                return PlayFrontError.Success;
            }

            if (Player.Game.Game.CurrentValue is null)
            {
                return PlayFrontError.InvalidOperation;
            }

            if (Player.Game.Game.CurrentValue.Value.Mode == GameMode.Challenge)
            {
                return PlayFrontError.Challenge;
            }

            if (!Player.Object.Alive.Alive.CurrentValue)
            {
                return PlayFrontError.Dead;
            }

            if (Player.GameManager.Status.CurrentValue.IsRuning())
            {
                return PlayFrontError.InvalidOperation;
            }

            await UnaryAsync(UnaryType.ReturnGame, default, default, default, cancellationToken);
            return PlayFrontError.Success;
        }

        public async Task<PlayFrontError> CancelGameAsync(CancellationToken cancellationToken)
        {
            if (Player.GameManager.Status.CurrentValue is null)
            {
                return PlayFrontError.Success;
            }

            if (Player.GameManager.Status.CurrentValue == GameManagerStatus.Return)
            {
                if (Player.Game.Game.CurrentValue is null)
                {
                    return PlayFrontError.InvalidOperation;
                }

                if (Player.Game.Game.CurrentValue.Value.Mode == GameMode.Challenge)
                {
                    return PlayFrontError.Challenge;
                }

                if (!Player.Object.Alive.Alive.CurrentValue)
                {
                    return PlayFrontError.Dead;
                }
            }

            if (!Player.GameManager.Status.CurrentValue.IsRuning())
            {
                return PlayFrontError.InvalidOperation;
            }

            await UnaryAsync(UnaryType.CancelGame, default, default, default, cancellationToken);
            return PlayFrontError.Success;
        }

        public async Task<PlayFrontError> PinSpecialtyAsync(SpecialtyId id, bool value, CancellationToken cancellationToken)
        {
            if (value && Player.Object.Specialties.PinnedCount >= PlayConst.SpecialtyMaxPinCount)
            {
                return PlayFrontError.ExceededMaximum;
            }

            await UnaryAsync(UnaryType.PinSpecialty, (int)id, value ? 1 : 0, default, cancellationToken);
            return PlayFrontError.Success;
        }

        public Task SetWelcomeAsync(CancellationToken cancellationToken)
        {
            return UnaryAsync(UnaryType.SetWelcome, default, default, default, cancellationToken);
        }

        private Task UnaryAsync(UnaryType type, int param1, int param2, int param3, CancellationToken cancellationToken)
        {
            return UnaryAsync(new Unary(IssueUnaryToken(), type, param1, param2, param3), cancellationToken);
        }

        private int IssueUnaryToken() => ++_unaryToken;

        private async Task UnaryAsync(Unary? unary, CancellationToken cancellationToken)
        {
            if (_unaryTcs is not null)
            {
                throw new InvalidOperationException();
            }

            _unaryRequest.Value = unary;
            _unaryTcs = new();

            using var _ = cancellationToken.Register(
                static state => ((TaskCompletionSource<bool>)state!).TrySetCanceled(),
                _unaryTcs);
            await _unaryTcs.Task;
        }

        private void CompleteUnaryTcs()
        {
            if (_unaryTcs is null)
            {
                return;
            }

            if (UnaryRequest.CurrentValue?.Token == Player.Unary.Result.CurrentValue?.Unary?.Token)
            {
                _unaryTcs.TrySetResult(true);
                _unaryTcs = null;
            }
        }
    }
}
