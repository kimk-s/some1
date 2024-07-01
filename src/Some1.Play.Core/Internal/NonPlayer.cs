using System;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class NonPlayer : INonPlayer
    {
        private readonly CharacterId _characterId;
        private readonly NonPlayerTiming _timing;
        private readonly byte _team;
        private readonly Object _object;
        private readonly Area _spot;
        private readonly ITime _time;
        private double _last;

        internal NonPlayer(
            CharacterId characterId,
            NonPlayerTiming timing,
            Area spot,
            byte team,
            IObjectFactory objectFactory,
            ITime time)
        {
            _characterId = characterId;
            _timing = timing;
            _spot = spot;
            _team = team;
            _object = objectFactory.CreateRoot(false);
            _object.Spot = _spot;
            _time = time;

            SetObject();
        }

        public IObject Object => _object;

        private void SetObject()
        {
            if (_object.Character.Id.CurrentValue is not null)
            {
                throw new InvalidOperationException();
            }

            _object.UpdateBefore += Object_UpdateCompleted;
            _object.SetCharacter(_characterId);
            _object.SetBattle(false);
            _object.SetTransfer(_spot.GetRandomPosition(), null);
            _object.SetTeamProperty(_team);
        }

        private void Object_UpdateCompleted(object? _, ParallelToken? e)
        {
            if (_object.Battle.Battle.CurrentValue is null)
            {
                throw new InvalidOperationException();
            }

            if (_object.Battle.Battle.CurrentValue.Value)
            {
                if (!_object.Alive.Alive.CurrentValue
                    && (!_object.Alive.Cycles.CanUpdate || _object.Alive.Cycles.Value.CurrentValue.B >= 1))
                {
                    StopObject(e);
                }
            }
            else
            {
                float seconds = _last == 0 ? _timing.DueTime : _timing.PeriodTime;
                if (_time.TotalSeconds - _last >= seconds)
                {
                    StartObject(e);
                }
            }
        }

        private void StartObject(ParallelToken? parallelToken)
        {
            _object.SetBattle(true);
            _object.SetSkin(SkinId.Skin0);
            if (_object.Character.Info!.Type.IsUnit())
            {
                _object.WarpTransfer(_object.Transform.Position.CurrentValue.B, parallelToken, false);
            }
        }

        private void StopObject(ParallelToken? parallelToken)
        {
            _object.SetCharacter(_characterId);
            _object.SetBattle(false);
            _object.SetTransfer(_spot.GetRandomPosition(), parallelToken);
            _object.SetSkin(null);
            _last = _time.TotalSeconds;
        }
    }
}
