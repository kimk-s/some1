using System;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal.Agents
{
    internal sealed class BattleAgent : IAgent
    {
        private readonly Object _object;
        private readonly ITime _time;

        private BattleCharacterAgentInfo? _info;

        private CastId? _castId;

        private const float SkipTime = 30;
        private double _skipLastTime;

        private const double LooseTimeMin = 0.3d;
        private const double LooseTimeMax = 0.5d;
        private const double LooseTimeRange = LooseTimeMax - LooseTimeMin;
        private double _looseEndTime;

        private const float WarpThresholdDistance = 60;

        private const float CastInterval = 2;
        private double _castTime;

        private const double IdleNoneTime = 6;
        private const double IdleMoveTime = 2;
        private bool _idleMove;
        private double _idleMoveTime;

        private bool _fight;

        static BattleAgent()
        {
            if (LooseTimeRange < 0)
            {
                throw new Exception($"LooseTimeRange is less then zero. (LooseTimeRange: {LooseTimeRange})");
            }
        }

        internal BattleAgent(Object @object, ITime time)
        {
            _object = @object;
            _time = time;
        }

        public void Set(ICharacterAgentInfo? info)
        {
            _info = (BattleCharacterAgentInfo)info!;
        }

        public void Set(Aim aim)
        {
            ThrowIfInfoIsNull();

            _object.SetAim(aim);
        }

        public void Update(float deltaSeconds, ParallelToken parallelToken)
        {
            ThrowIfInfoIsNull();

            if (!CanUpdateByObjectState())
            {
                return;
            }

            if (TrySkip(parallelToken))
            {
                return;
            }

            if (IsInLoose())
            {
                return;
            }

            if (!IsCastCurrentIsNull())
            {
                return;
            }

            ThrowIfInvalidAttackCastItem();

            WarpSpotIfTooFar(parallelToken);

            UpdateCastId();

            if (UpdateCastScan(parallelToken))
            {
                return;
            }

            UpdateDamageFight();

            UpdateWalkScan(parallelToken);

            UpdateIdle();
        }

        private void ThrowIfInfoIsNull()
        {
            if (_info is null)
            {
                throw new InvalidOperationException();
            }
        }

        private bool CanUpdateByObjectState()
        {
            return _object.Battle.Battle.CurrentValue == true && _object.Alive.Alive.CurrentValue;
        }

        private bool TrySkip(ParallelToken parallelToken)
        {
            double lastTime = _skipLastTime;
            _skipLastTime = _time.TotalSeconds;

            if (lastTime == 0)
            {
                return false;
            }

            double deltaTime = _time.TotalSeconds - lastTime;
            if (deltaTime < SkipTime)
            {
                return false;
            }

            Stop();
            WarpSpot(parallelToken, false);

            return true;
        }
        
        private bool IsInLoose()
        {
            bool result = _looseEndTime > _time.TotalSeconds;
            if (!result)
            {
                double looseTime = LooseTimeMin + LooseTimeRange * RandomForUnity.NextSingle();
                _looseEndTime = _time.TotalSeconds + looseTime;
            }
            return result;
        }

        private bool IsCastCurrentIsNull()
        {
            return _object.Cast.Current.CurrentValue is null;
        }

        private void ThrowIfInvalidAttackCastItem()
        {
            if ((_object.Cast.Items[CastId.Attack].ObjectTarget.Team.Info & TeamTargetInfo.Enemy) == 0)
            {
                throw new InvalidOperationException();
            }
        }

        private void WarpSpotIfTooFar(ParallelToken parallelToken)
        {
            if (_object.Spot is null)
            {
                return;
            }

            var delta = _object.Transform.Position.CurrentValue.B - _object.Transfer.LastWarpPosition;
            if (delta.LengthSquared() < MathF.Pow(WarpThresholdDistance, 2))
            {
                return;
            }

            Stop();
            WarpSpot(parallelToken);
        }

        private void WarpSpot(ParallelToken parallelToken, bool addWarpOutEffect = true)
        {
            if (_object.Spot is null)
            {
                return;
            }

            _object.WarpTransfer(_object.Spot!.Value.GetRandomPosition(), parallelToken, addWarpOutEffect);
        }

        private void UpdateCastId()
        {
            if (_castTime > _time.TotalSeconds)
            {
                return;
            }
            _castTime = _time.TotalSeconds + CastInterval * RandomForUnity.NextSingle();

            _castId = NewCastId();
        }

        private bool UpdateCastScan(ParallelToken parallelToken)
        {
            if (_castId is null)
            {
                throw new InvalidOperationException();
            }

            if (!_object.TrySetCastScan(_castId.Value, parallelToken))
            {
                return false;
            }

            var castItem = _object.Cast.Items[_castId.Value];
            if (!castItem.CanConsume())
            {
                _object.SetTransformRotation(_object.Cast.Next!.Value.Aim.Rotation);
            }

            _object.SetWalk(null);
            if ((castItem.ObjectTarget.Team.Info & TeamTargetInfo.Enemy) != 0)
            {
                _fight = true;
            }
            return true;
        }

        private void UpdateDamageFight()
        {
            if (_fight)
            {
                return;
            }

            for (int i = 0; i < _object.Hits.Count; i++)
            {
                if (_object.Hits[i].Id?.IsDamage() == true)
                {
                    _fight = true;
                    break;
                }
            }
        }

        private void UpdateWalkScan(ParallelToken parallelToken)
        {
            if (_castId is null)
            {
                throw new InvalidOperationException();
            }

            var castItem = _object.Cast.Items[_castId.Value];

            if (_object.TrySetWalkScan(GetWalkScanLength(), castItem.ObjectTarget, parallelToken))
            {
                if ((castItem.ObjectTarget.Team.Info & TeamTargetInfo.Enemy) != 0)
                {
                    _fight = true;
                }
            }
            else
            {
                if ((castItem.ObjectTarget.Team.Info & TeamTargetInfo.Enemy) != 0)
                {
                    _fight = false;
                }
            }
        }

        private void UpdateIdle()
        {
            if (_fight)
            {
                return;
            }

            if (_idleMove)
            {
                if (_idleMoveTime > _time.TotalSeconds)
                {
                    return;
                }

                StopIdleMove();
            }
            else
            {
                if (_idleMoveTime == 0)
                {
                    StopIdleMove();
                }
                else
                {
                    if (_idleMoveTime > _time.TotalSeconds)
                    {
                        return;
                    }

                    StartIdleMove();
                }
            }
        }

        private void StartIdleMove()
        {
            _idleMove = true;
            _idleMoveTime = _time.TotalSeconds + IdleMoveTime * RandomForUnity.NextSingle();
            _object.SetWalk(new(true, 360 * RandomForUnity.NextSingle()));
        }

        private void StopIdleMove()
        {
            _idleMove = false;
            _idleMoveTime = _time.TotalSeconds + IdleNoneTime * RandomForUnity.NextSingle();
            _object.SetWalk(null);
        }

        public void Stop()
        {
            _castId = null;
            _looseEndTime = 0;
            _castTime = 0;
            _idleMove = false;
            _idleMoveTime = 0;
            _fight = false;
        }

        public void Reset()
        {
            Stop();
            _skipLastTime = 0;
            _info = null;
        }

        private float GetWalkScanLength()
        {
            if (_info is null)
            {
                throw new InvalidOperationException();
            }

            return _fight ? _info.FightWalkScanLength : _info.NotFightWalkScanLength;
        }

        private CastId NewCastId()
        {
            if (_info is null)
            {
                throw new InvalidOperationException();
            }

            if (_object.Cast.Items[CastId.Super].CanConsume()
                && _info.SuperProbability > RandomForUnity.NextSingle())
            {
                return CastId.Super;
            }

            if (_object.Cast.Items[CastId.Ultra].CanConsume()
                && _info.UltraProbability > RandomForUnity.NextSingle())
            {
                return CastId.Ultra;
            }

            return CastId.Attack;
        }
    }
}
