using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayFront
    {
        ReadOnlyReactiveProperty<PipeState> PipeState { get; }
        ReadOnlyReactiveProperty<Unary?> UnaryRequest { get; }
        ReadOnlyReactiveProperty<bool> IsUnaryRunning { get; }
        ReadOnlyReactiveProperty<Cast?> CastRequest { get; }
        ReadOnlyReactiveProperty<Walk?> MoveRequest { get; }
        ReadOnlyReactiveProperty<int> AttackFailNotAnyLoadCount { get; }
        IPlayerFront Player { get; }
        IRankingGroupFront Rankings { get; }
        IObjectGroupFront Objects { get; }
        IReadOnlyList<IFloorFront> Floors { get; }
        IRegionGroupFront Regions { get; }
        ISpaceFront Space { get; }
        ITimeFront Time { get; }
        ReadOnlyReactiveProperty<float> LocalTimeScale { get; }

        Task StartPipeAsync(CancellationToken cancellationToken);
        PlayFrontError GetExitError();
        PlayFrontError Exit();
        void FinishPipe();
        void UpdatePipe(float deltaSeconds);
        void Cast(Cast cast);
        void Walk(Walk move);
        Task ClearUnaryAsync(CancellationToken cancellationToken);
        Task<PlayFrontError> SelectCharacterAsync(CharacterId characterId, CancellationToken cancellationToken);
        Task<PlayFrontError> SetCharacterIsRandomSkinAsync(CharacterId id, bool value, CancellationToken cancellationToken);
        Task<PlayFrontError> SelectCharacterSkinAsync(CharacterId id, SkinId skinId, CancellationToken cancellationToken);
        Task<PlayFrontError> SetCharacterSkinIsRandomSelectedAsync(CharacterId id, SkinId skinId, bool value, CancellationToken cancellationToken);
        Task SetEmojiAsync(EmojiId id, CancellationToken cancellationToken);
        Task SelelctGameArgsAsync(GameMode id, CancellationToken cancellationToken);
        Task ReadyGameAsync(CancellationToken cancellationToken);
        Task<PlayFrontError> ReturnGameAsync(CancellationToken cancellationToken);
        Task<PlayFrontError> CancelGameAsync(CancellationToken cancellationToken);
        Task<PlayFrontError> PinSpecialtyAsync(SpecialtyId id, bool value, CancellationToken cancellationToken);
        Task SetWelcomeAsync(CancellationToken cancellationToken);
    }
}
