using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public interface IPlayerFront
    {
        IPlayerUnaryFront Unary { get; }
        IPlayerGameManagerFront GameManager { get; }
        IPlayerGameArgsGroupFront GameArgses { get; }
        IPlayerGameFront Game { get; }
        IPlayerGameResultGroupFront GameResults { get; }
        IPlayerGameToastFront GameToast { get; }
        IPlayerExpFront Exp { get; }
        IPlayerPremiumFront Premium { get; }
        IPlayerCharacterGroupFront Characters { get; }
        IPlayerSpecialtyGroupFront Specialties { get; }
        IPlayerTitleFront Title { get; }
        IPlayerEmojiGroupFront Emojis { get; }
        IPlayerWelcomeFront Welcome { get; }
        IReadOnlyDictionary<SeasonId, IPlayerSeasonFront> Seasons { get; }
        IPlayerObjectFront Object { get; }
    }
}
