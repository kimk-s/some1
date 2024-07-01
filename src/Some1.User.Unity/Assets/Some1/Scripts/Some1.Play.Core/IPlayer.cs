using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IPlayer
    {
        string? UserId { get; }
        PipeState PipeState { get; }
        PlayerDataStatus DataStatus { get; }
        bool Manager { get; }
        IPlayerUnary Unary { get; }
        IPlayerGameManager GameManager { get; }
        IPlayerGameArgsGroup GameArgses { get; }
        IPlayerGame Game { get; }
        IPlayerGameResultGroup GameResults { get; }
        IPlayerExp Exp { get; }
        IPlayerPremium Premium { get; }
        IPlayerCharacterGroup Characters { get; }
        IReadOnlyDictionary<SpecialtyId, IPlayerSpecialty> Specialties { get; }
        IPlayerTitle Title { get; }
        IPlayerEmoji Emoji { get; }
        IPlayerWelcome Welcome { get; }
        ILeader Leader { get; }
        IObject Object { get; }
    }
}
