using System;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class PlayerIdManager
    {
        private readonly ITime _time;
        private readonly PlayerIdSet[] _sets = new PlayerIdSet[2];
        private PlayerIdSet _primarySet;
        private PlayerIdSet _secondarySet;
        private DateTime _swappedTime;

        public PlayerIdManager(ITime time)
        {
            for (int i = 0; i < _sets.Length; i++)
            {
                _sets[i] = new();
            }
            _primarySet = _sets[0];
            _secondarySet = _sets[1];
            _time = time;
            _swappedTime = _time.UtcNow;
        }

        internal bool Contains(PlayerId playerId)
        {
            foreach (var item in _sets)
            {
                if (item.Contains(playerId))
                {
                    return true;
                }
            }
            return false;
        }

        internal void Add(string userId, PlayerId playerId)
        {
            if (_secondarySet.Contains(playerId))
            {
                throw new InvalidOperationException();
            }
            _primarySet.Add(userId, playerId);
        }

        internal bool Remove(string userId, out PlayerId playerId)
        {
            foreach (var item in _sets)
            {
                if (item.Remove(userId, out playerId))
                {
                    return true;
                }
            }
            playerId = default;
            return false;
        }

        internal void Update()
        {
            const double Time = 60 * 60;
            const int Count = 10_000;

            if ((_time.UtcNow - _swappedTime).TotalSeconds > Time || _primarySet.Count > Count)
            {
                Swap();
            }
        }

        private void Swap()
        {
            _secondarySet.Clear();
            (_secondarySet, _primarySet) = (_primarySet, _secondarySet);

            _swappedTime = _time.UtcNow;
        }
    }
}
