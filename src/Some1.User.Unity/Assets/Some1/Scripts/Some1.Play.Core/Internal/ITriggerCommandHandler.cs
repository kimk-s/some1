using System;
using System.Numerics;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal interface ITriggerCommandHandler
    {
        bool Handle(ITriggerCommandInfo info, Object destination, ParallelToken parallelToken);
    }

    internal sealed class TriggerCommandHandler : ITriggerCommandHandler
    {
        private readonly TriggerEventSourceState _sourceState;
        private readonly int _selfObjectId;

        internal TriggerCommandHandler(TriggerEventSourceState sourceState, int selfObjectId)
        {
            _sourceState = sourceState;
            _selfObjectId = selfObjectId;
        }

        private TriggerSourceState Source => _sourceState.Source;

        private TriggerEventState Event => _sourceState.Event;

        public bool Handle(ITriggerCommandInfo info, Object destination, ParallelToken parallelToken) => info switch
        {
            SetShiftCommandInfo i => HandleSetShiftCommandInfo(i, destination, parallelToken),
            AddBuffCommandInfo i => HandleAddBuffCommandInfo(i, destination, parallelToken),
            AddBoosterCommandInfo i => HandleAddBoosterCommandInfo(i, destination, parallelToken),
            AddSpecialtyCommandInfo i => HandleAddSpecialtyCommandInfo(i, destination, parallelToken),
            AddHitCommandInfo i => HandleAddHitCommandInfo(i, destination, parallelToken),
            AddEnergyCommandInfo i => HandleAddEnergyCommandInfo(i, destination, parallelToken),
            AddChildCommandInfo i => HandleAddChildCommandInfo(i, destination, parallelToken),
            AddMoveCommandInfo i => HandleAddMoveCommandInfo(i, destination, parallelToken),
            SetCharacterCommandInfo i => HandleSetCharacterCommandInfo(i, destination, parallelToken),
            _ => throw new InvalidOperationException($"Invalid TriggerCommandInfo Type: {info.GetType().Name}"),
        };

        private static float GetAngle(Area a, Area b) => a == b ? PlayConst.HitAngleQuiet : Vector2Helper.AngleBetween(a.Position, b.Position);

        private bool IsSelf(Object destination) => destination.Id == _selfObjectId;

        private bool HandleSetShiftCommandInfo(SetShiftCommandInfo info, Object destination, ParallelToken parallelToken)
        {
            return Handle(
                info.Id,
                info.Rotation.Mix(info.Id != ShiftId.Knock || Source.Anchor is null
                    ? Source.Aim.Rotation
                    : Vector2Helper.AngleBetween(Source.Anchor.Value, destination.Properties.Area.Position)),
                info.Distance?.Mix(Source.Aim.Length) ?? SpeedHelper.GetDistance(info.Time!.Value, info.Speed!.Value),
                info.Time ?? SpeedHelper.GetTime(info.Distance!.Value.Mix(Source.Aim.Length), info.Speed!.Value),
                info.Speed ?? SpeedHelper.GetSpeed(info.Distance!.Value.Mix(Source.Aim.Length), info.Time!.Value),
                destination,
                IsSelf(destination),
                parallelToken);

            static bool Handle(ShiftId id, float rotation, float distance, float time, float speed, Object destination, bool self, ParallelToken parallelToken)
            {
                return self
                    ? destination.SetShift(id, rotation, distance, time, speed)
                    : destination.Messages.SetShift(id, rotation, distance, time, speed, parallelToken);
            }
        }

        private bool HandleAddBuffCommandInfo(AddBuffCommandInfo info, Object destination, ParallelToken parallelToken)
        {
            return Handle(
                info.Id,
                Source.RootId,
                GetAngle(Source.Area, destination.Properties.Area),
                Source.SkinId!.Value,
                Source.OffenseStat,
                Source.Trait,
                Source.Team,
                destination,
                IsSelf(destination),
                parallelToken);

            static bool Handle(BuffId id, int rootId, float angle, SkinId skinId, int offeceStat, Trait traitId, byte team, Object destination, bool self, ParallelToken parallelToken)
            {
                return self
                    ? destination.AddBuff(id, rootId, angle, skinId, offeceStat, traitId, team)
                    : destination.Messages.AddBuff(id, rootId, angle, skinId, offeceStat, traitId, team, parallelToken);
            }
        }

        private bool HandleAddBoosterCommandInfo(AddBoosterCommandInfo info, Object destination, ParallelToken parallelToken)
        {
            return Handle(
                info.Id,
                1,
                destination,
                IsSelf(destination),
                parallelToken);

            static bool Handle(BoosterId id, int number, Object destination, bool self, ParallelToken parallelToken)
            {
                return self
                    ? destination.AddBooster(id, number)
                    : destination.Messages.AddBooster(id, number, parallelToken);
            }
        }

        private bool HandleAddSpecialtyCommandInfo(AddSpecialtyCommandInfo info, Object destination, ParallelToken parallelToken)
        {
            return Handle(info.Id, 1, destination, IsSelf(destination), parallelToken);

            static bool Handle(SpecialtyId id, int number, Object destination, bool self, ParallelToken parallelToken)
            {
                return self
                    ? destination.AddSpecialty(id, number)
                    : destination.Messages.AddSpecialty(id, number, parallelToken);
            }
        }

        private bool HandleAddHitCommandInfo(AddHitCommandInfo info, Object destination, ParallelToken parallelToken)
        {
            return Handle(
                info.Id,
                StatHelper.CalculateDamage(info.BaseValue, Source.OffenseStat, destination.Stats[StatId.Defense].Value),
                Source.RootId,
                GetAngle(Source.Area, destination.Properties.Area),
                Source.BirthId,
                destination,
                IsSelf(destination),
                parallelToken);

            static bool Handle(HitId id, int value, int rootId, float angle, BirthId? birthId, Object destination, bool self, ParallelToken parallelToken)
            {
                return self
                    ? destination.AddHit(id, value, rootId, angle, birthId)
                    : destination.Messages.AddHit(id, value, rootId, angle, birthId, parallelToken);
            }
        }

        private bool HandleAddEnergyCommandInfo(AddEnergyCommandInfo i, Object destination, ParallelToken parallelToken)
        {
            return Handle(
                i.Id, 
                StatHelper.Calculate(i.Value, Source.OffenseStat),
                destination,
                IsSelf(destination),
                parallelToken);

            static bool Handle(EnergyId id, int value, Object destination, bool self, ParallelToken parallelToken)
            {
                return self
                    ? destination.AddEnergy(id, value)
                    : destination.Messages.AddEnergy(id, value, parallelToken);
            }
        }

        private bool HandleAddChildCommandInfo(AddChildCommandInfo i, Object destination, ParallelToken parallelToken)
        {
            return Handle(
                i.CharacterId,
                Source.SkinId,
                Source.Team,
                Source.OffenseStat,
                PositionHelper.From(i.Position, Source.Aim, Source.Area.Position)
                    + (i.RandomPositionRadius > 0 ? new Aim(360 * RandomForUnity.NextSingle(), i.RandomPositionRadius * RandomForUnity.NextSingle()).ToVector2() : Vector2.Zero),
                AimHelper.From(i.Aim, Source.Aim),
                destination,
                IsSelf(destination),
                parallelToken);

            static bool Handle(
                CharacterId characterId,
                SkinId? skinId,
                byte team,
                int power,
                Vector2 position,
                Aim aim,
                Object destination,
                bool self,
                ParallelToken parallelToken)
            {
                return self
                    ? destination.AddChild(characterId, skinId, team, power, position, aim, parallelToken)
                    : destination.Messages.AddChild(characterId, skinId, team, power, position, aim, parallelToken);
            }
        }

        private bool HandleAddMoveCommandInfo(AddMoveCommandInfo i, Object destination, ParallelToken parallelToken)
        {
            return Handle(
                MoveHelper.CalculateVelocity(i.Rotation + Source.Aim.Rotation, i.Speed),
                destination,
                IsSelf(destination),
                parallelToken);

            static bool Handle(Vector2 velocity, Object destination, bool self, ParallelToken parallelToken)
            {
                return self
                    ? destination.AddMove(velocity)
                    : destination.Messages.AddMove(velocity, parallelToken);
            }
        }

        private bool HandleSetCharacterCommandInfo(SetCharacterCommandInfo i, Object destination, ParallelToken parallelToken)
        {
            return Handle(
                i.CharacterId,
                destination,
                IsSelf(destination),
                parallelToken);

            static bool Handle(CharacterId id, Object destination, bool self, ParallelToken parallelToken)
            {
                return self
                    ? destination.SetCharacter(id)
                    : destination.Messages.SetCharacter(id, parallelToken);
            }
        }
    }
}
