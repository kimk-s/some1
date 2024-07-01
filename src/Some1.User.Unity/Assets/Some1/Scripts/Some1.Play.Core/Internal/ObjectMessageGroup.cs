using System;
using System.Numerics;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectMessageGroup
    {
        private readonly Object _object;
        private readonly ITime _time;
        private readonly ParallelCollection<ObjectMessage>[] _collections;

        internal ObjectMessageGroup(
            ParallelOptions options,
            Object @object,
            ITime time)
        {
            _object = @object;
            _time = time;
            _collections = new ParallelCollection<ObjectMessage>[2];
            for (var i = 0; i < _collections.Length; i++)
            {
                _collections[i] = new(options);
            }
        }

        private ParallelCollection<ObjectMessage> Reader => _collections[_time.FrameCount % 2];

        private ParallelCollection<ObjectMessage> Writer => _collections[(_time.FrameCount % 2 == 1) ? 0 : 1];

        private int FrameCount => _time.FrameCount;

        internal bool SetShift(ShiftId id, float rotation, float distance, float time, float speed, ParallelToken? parallelToken)
        {
            if (!_object.CanSetShift)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.SetShift,
                    intParam1: (int)id,
                    floatParam1: rotation,
                    floatParam2: distance,
                    floatParam3: time,
                    floatParam4: speed),
                parallelToken);

            return true;
        }

        internal bool AddBuff(BuffId id, int rootId, float angle, SkinId skinId, int offenseStat, Trait traitId, byte team, ParallelToken? parallelToken)
        {
            if (!_object.CanAddBuff)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.AddBuff,
                    (int)id,
                    rootId,
                    (int)skinId,
                    offenseStat,
                    (int)traitId,
                    team,
                    angle),
                parallelToken);

            return true;
        }

        internal bool AddBooster(BoosterId id, int number, ParallelToken? parallelToken)
        {
            if (!_object.CanAddBooster)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.AddBooster,
                    (int)id,
                    number),
                parallelToken);

            return true;
        }

        internal bool AddSpecialty(SpecialtyId id, int number, ParallelToken? parallelToken)
        {
            if (!_object.CanAddSpecialty)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.AddSpecialty,
                    (int)id,
                    number),
                parallelToken);

            return true;
        }

        internal bool AddHit(HitId id, int value, int rootId, float angle, BirthId? birthId, ParallelToken? parallelToken)
        {
            if (!_object.CanAddHit)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.AddHit,
                    (int)id,
                    value,
                    rootId,
                    birthId?.ParentId ?? -1,
                    birthId?.FrameCount ?? -1,
                    floatParam1: angle),
                parallelToken);

            return true;
        }

        internal bool AddEnergy(EnergyId id, int value, ParallelToken? parallelToken)
        {
            if (!_object.CanAddEnergy)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.AddEnergy,
                    (int)id,
                    value),
                parallelToken);

            return true;
        }

        internal bool AddChild(
            CharacterId characterId,
            SkinId? skinId,
            byte team,
            int power,
            Vector2 position,
            Aim aim,
            ParallelToken? parallelToken)
        {
            if (!_object.CanAddChild)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.AddChild,
                    (int)characterId,
                    skinId is null ? -1 : (int)skinId,
                    team,
                    power,
                    floatParam1: position.X,
                    floatParam2: position.Y,
                    floatParam3: aim.Rotation,
                    floatParam4: aim.Length),
                parallelToken);

            return true;
        }

        internal bool AddMove(Vector2 velocity, ParallelToken? parallelToken)
        {
            if (!_object.CanAddMove)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.AddMove,
                    floatParam1: velocity.X,
                    floatParam2: velocity.Y),
                parallelToken);

            return true;
        }

        internal bool AddLike(Like like, ParallelToken? parallelToken)
        {
            if (!_object.CanAddLike)
            {
                return false;
            }

            Write(
                new(
                    ObjectMessageType.AddLike,
                    like.PlayerId),
                parallelToken);

            return true;
        }

        internal bool SetCharacter(CharacterId id, ParallelToken? parallelToken)
        {
            Write(
                new(
                    ObjectMessageType.SetCharacter,
                    (int)id),
                parallelToken);

            return true;
        }

        internal void Update(ParallelToken? parallelToken)
        {
            foreach (var item in Reader)
            {
                if (item.FrameCount != _time.FrameCount - 1)
                {
                    continue;
                }

                switch (item.Body.Type)
                {
                    case ObjectMessageType.SetShift:
                        ExecuteSetShift(item.Body);
                        break;
                    case ObjectMessageType.AddBuff:
                        ExecuteAddBuff(item.Body);
                        break;
                    case ObjectMessageType.AddBooster:
                        ExecuteAddBooster(item.Body);
                        break;
                    case ObjectMessageType.AddSpecialty:
                        ExecuteAddSpecialty(item.Body);
                        break;
                    case ObjectMessageType.AddHit:
                        ExecuteAddHit(item.Body);
                        break;
                    case ObjectMessageType.AddEnergy:
                        ExecuteAddEnergy(item.Body);
                        break;
                    case ObjectMessageType.AddChild:
                        ExecuteAddChild(item.Body, parallelToken);
                        break;
                    case ObjectMessageType.AddMove:
                        ExecuteAddMove(item.Body);
                        break;
                    case ObjectMessageType.AddLike:
                        ExecuteAddLike(item.Body);
                        break;
                    case ObjectMessageType.SetCharacter:
                        ExecuteSetCharacter(item.Body);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            Reader.Clear();
        }

        internal void Reset(bool onlyReader)
        {
            Clear();
            if (!onlyReader)
            {
                Writer.Clear();
            }
        }

        internal void Clear()
        {
            Reader.Clear();
        }

        private void Write(in ObjectMessageBody body, ParallelToken? parallelToken)
        {
            if (!Writer.Add(new(body, FrameCount), parallelToken))
            {
                Console.WriteLine($"Failed to add message in ObjectMessageGroup.Write.");
            }
        }

        private void ExecuteSetShift(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.SetShift)
            {
                throw new InvalidOperationException();
            }

            _object.SetShift(
                (ShiftId)body.IntParam1,
                body.FloatParam1,
                body.FloatParam2,
                body.FloatParam3,
                body.FloatParam4);
        }

        private void ExecuteAddBuff(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.AddBuff)
            {
                throw new InvalidOperationException();
            }

            _object.AddBuff(
                (BuffId)body.IntParam1,
                body.IntParam2,
                body.FloatParam1,
                (SkinId)body.IntParam3,
                body.IntParam4,
                (Trait)body.IntParam5,
                checked((byte)body.IntParam6));
        }

        private void ExecuteAddBooster(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.AddBooster)
            {
                throw new InvalidOperationException();
            }

            _object.AddBooster(
                (BoosterId)body.IntParam1,
                body.IntParam2);
        }

        private void ExecuteAddSpecialty(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.AddSpecialty)
            {
                throw new InvalidOperationException();
            }

            _object.AddSpecialty(
                (SpecialtyId)body.IntParam1,
                body.IntParam2);
        }

        private void ExecuteAddHit(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.AddHit)
            {
                throw new InvalidOperationException();
            }

            _object.AddHit(
                (HitId)body.IntParam1,
                body.IntParam2,
                body.IntParam3,
                body.FloatParam1,
                body.IntParam4 == -1 ? null : new(body.IntParam4, body.IntParam5));
        }

        private void ExecuteAddEnergy(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.AddEnergy)
            {
                throw new InvalidOperationException();
            }

            _object.AddEnergy(
                (EnergyId)body.IntParam1,
                body.IntParam2);
        }

        private void ExecuteAddChild(in ObjectMessageBody body, ParallelToken? parallelToken)
        {
            if (body.Type != ObjectMessageType.AddChild)
            {
                throw new InvalidOperationException();
            }

            _object.AddChild(
                (CharacterId)body.IntParam1,
                body.IntParam2 == -1 ? null : (SkinId)body.IntParam2,
                checked((byte)body.IntParam3),
                body.IntParam4,
                new(body.FloatParam1, body.FloatParam2),
                new(body.FloatParam3, body.FloatParam4),
                parallelToken);
        }

        private void ExecuteAddMove(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.AddMove)
            {
                throw new InvalidOperationException();
            }

            _object.AddMove(new(body.FloatParam1, body.FloatParam2));
        }

        private void ExecuteAddLike(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.AddLike)
            {
                throw new InvalidOperationException();
            }

            _object.AddLike(new((PlayerId)body.IntParam1));
        }

        private void ExecuteSetCharacter(in ObjectMessageBody body)
        {
            if (body.Type != ObjectMessageType.SetCharacter)
            {
                throw new InvalidOperationException();
            }

            _object.SetCharacter((CharacterId)body.IntParam1);
        }
    }
}
