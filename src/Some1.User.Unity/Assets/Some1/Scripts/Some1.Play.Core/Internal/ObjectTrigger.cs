using System;
using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed class ObjectTrigger : IDisposable
    {
        private readonly TriggerInfoGroup _infos;
        private readonly IObject _object;
        private readonly ObjectHierarchy _hierarchy;
        private readonly Space _space;
        private readonly List<Trigger> _triggers = new();

        internal ObjectTrigger(TriggerInfoGroup infos, Object @object, Space space)
        {
            _infos = infos;
            _object = @object;
            _hierarchy = @object.Hierarchy;
            _space = space;

            _triggers.AddRange(new Trigger[]
            {
                CreateAlive(),
                CreateIdle(),
                CreateShift(),
                CreateCast(),
                CreateHitReached(),
                CreateEnergyReached(),
                CreateTransferMoveFailed(),
            });
            _triggers.AddRange(CreateBuffGroup());
        }

        public void Dispose()
        {
            foreach (var item in _triggers)
            {
                item.Dispose();
            }
        }

        internal bool GetStuffAcquired(int objectId)
        {
            return _triggers[0].ContainsDestinationUnique(TriggerDestinationUniqueId.Scoped1, objectId);
        }

        internal void Reset()
        {
            foreach (var item in _triggers)
            {
                item.Reset();
            }
        }

        private Trigger CreateAlive() => Create(
            () => new CharacterTriggerId(_object.Character.Id.CurrentValue!.Value, _object.Alive.Alive.CurrentValue
                ? CharacterTriggerType.AliveTrue
                : CharacterTriggerType.AliveFalse),
            new CycleTriggerEventManager(
                _object.Alive.Cycles,
               state => state.Set(_object)));

        private Trigger CreateIdle() => Create(
            () => new CharacterTriggerId(_object.Character.Id.CurrentValue!.Value, _object.Idle.Idle
                ? CharacterTriggerType.IdleTrue
                : CharacterTriggerType.IdleFalse),
            new CycleTriggerEventManager(
                _object.Idle.Cycles,
               state => state.Set(_object)));

        private Trigger CreateShift() => Create(
            () => _object.Shift.Shift.CurrentValue is null
                ? null
                : new CharacterTriggerId(_object.Character.Id.CurrentValue!.Value, _object.Shift.Shift.CurrentValue.Value.Id switch
                {
                    ShiftId.Jump => CharacterTriggerType.ShiftJump,
                    ShiftId.Knock => CharacterTriggerType.ShiftKnock,
                    ShiftId.Dash => CharacterTriggerType.ShiftDash,
                    ShiftId.Grab => CharacterTriggerType.ShiftGrab,
                    _ => throw new InvalidOperationException()
                }),
            new CycleTriggerEventManager(
                _object.Shift.Cycles,
                state => state.Set(_object).Set(_object.Shift)));

        private Trigger CreateCast() => Create(
            () => _object.Cast.Current.CurrentValue is null
                ? null
                : new CharacterTriggerId(_object.Character.Id.CurrentValue!.Value, _object.Cast.Current.CurrentValue.Value.Id switch
                {
                    CastId.Attack => CharacterTriggerType.CastAttack,
                    CastId.Super => CharacterTriggerType.CastSuper,
                    CastId.Ultra => CharacterTriggerType.CastUltra,
                    _ => throw new InvalidOperationException()
                }),
            new CycleTriggerEventManager(
                _object.Cast.Cycles,
               state => state.Set(_object).Set(_object.Cast)));

        private Trigger CreateHitReached() => Create(
            () => new CharacterTriggerId(_object.Character.Id.CurrentValue!.Value, CharacterTriggerType.HitReached),
            new EmptyTriggerEventManager(
                _object.HitReached,
               state => state.Set(_object)));

        private Trigger CreateEnergyReached() => Create(
            () => new CharacterTriggerId(_object.Character.Id.CurrentValue!.Value, CharacterTriggerType.EnergyReached),
            new EmptyTriggerEventManager(
                _object.EnergyReached,
               state => state.Set(_object)));

        private Trigger CreateTransferMoveFailed() => Create(
            () => new CharacterTriggerId(_object.Character.Id.CurrentValue!.Value, CharacterTriggerType.TransferMoveFailed),
            new EmptyTriggerEventManager(
                _object.Transfer.MoveFailed,
                state => state.Set(_object)));

        private Trigger[] CreateBuffGroup()
        {
            var buffs = new Trigger[_object.Buffs.Count];
            for (int i = 0; i < _object.Buffs.Count; i++)
            {
                buffs[i] = CreateBuff(_object.Buffs[i]);
            }
            return buffs;
        }

        private Trigger CreateBuff(IObjectBuff buff) => Create(
            () => buff.Id is null ? null : new BuffTriggerId(buff.Id!.Value),
            new CycleTriggerEventManager(
                buff.Cycles,
                state => state.Set(_object).Set(buff)));

        private Trigger Create(Func<TriggerId?> getId, ITriggerEventManager eventManager) => new(
            () => _infos.ById.GetValueOrDefault(getId()),
            eventManager,
            _hierarchy,
            _space);
    }

    internal static class TriggerSourceStateExtensions
    {
        internal static TriggerSourceState Set(this TriggerSourceState state, IObject @object)
        {
            state.RootId = @object.Hierarchy.Root.Id;
            state.SkinId = @object.Skin.Id;
            state.OffenseStat = @object.Stats[StatId.Offense].Value;
            state.Trait = @object.Trait.Value;
            state.NextTrait = @object.Trait.Next.CurrentValue;
            state.Team = @object.Properties.Team.CurrentValue;
            state.Area = @object.Properties.Area;
            state.Aim = @object.Properties.Aim;
            state.Anchor = @object.Properties.Anchor;
            state.BirthId = @object.Properties.BirthId;
            return state;
        }

        internal static TriggerSourceState Set(this TriggerSourceState state, IObjectShift shift)
        {
            if (shift.Shift.CurrentValue is not null)
            {
                state.Aim = new(shift.Shift.CurrentValue.Value.Rotation, shift.Shift.CurrentValue.Value.Distance);
            }
            return state;
        }

        internal static TriggerSourceState Set(this TriggerSourceState state, IObjectCast cast)
        {
            if (cast.Current.CurrentValue is not null)
            {
                state.Aim = cast.Current.CurrentValue.Value.Aim;
            }
            return state;
        }

        internal static TriggerSourceState Set(this TriggerSourceState state, IObjectBuff buff)
        {
            state.RootId = buff.RootId;
            state.SkinId = buff.SkinId;
            state.OffenseStat = buff.OffenseStat;
            state.Trait = buff.Trait;
            state.NextTrait = Trait.None;
            state.Team = buff.Team;
            //state.PlayerId = 0;
            return state;
        }
    }
}
