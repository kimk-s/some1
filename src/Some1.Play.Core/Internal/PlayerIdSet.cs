using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerIdSet
    {
        private readonly Dictionary<string, PlayerId> _userIdToPlayerId = new();
        private readonly HashSet<PlayerId> _playerIds = new();

        internal int Count => _userIdToPlayerId.Count;

        internal bool Contains(PlayerId playerId)
        {
            return _playerIds.Contains(playerId);
        }

        internal void Add(string userId, PlayerId playerId)
        {
            _userIdToPlayerId.Add(userId, playerId);
            _playerIds.Add(playerId);
        }

        internal bool Remove(string userId, out PlayerId playerId)
        {
            if (!_userIdToPlayerId.Remove(userId, out playerId))
            {
                return false;
            }

            _playerIds.Remove(playerId);
            return true;
        }

        internal void Clear()
        {
            _userIdToPlayerId.Clear();
            _playerIds.Clear();
        }
    }
}
