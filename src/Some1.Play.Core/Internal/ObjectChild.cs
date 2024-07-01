using System;
using System.Numerics;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal interface IObjectChild
    {
        IObject Object { get; }
    }

    internal sealed class ObjectChild : IObjectChild
    {
        private readonly ITime _time;

        internal ObjectChild(Object parent, IObjectFactory objectFactory, ITime time)
        {
            Object = objectFactory.CreateChild(parent);
            Object.UpdateBefore += Object_UpdateBefore;
            _time = time;
        }

        IObject IObjectChild.Object => Object;

        internal Object Object { get; }

        internal double TotalSecondsSet { get; private set; }

        internal bool TryTakeControlForSet()
        {
            return Object.Character.Id.CurrentValue is null && Object.TryTakeControl();
        }

        internal void Set(
            CharacterId characterId,
            SkinId? skinId,
            byte team,
            int power,
            Vector2 position,
            Aim aim,
            BirthId? birthId,
            ParallelToken? parallelToken)
        {
            if (Object.Character.Id.CurrentValue is not null)
            {
                throw new InvalidOperationException();
            }

            Object.SetCharacter(characterId);
            Object.SetBattle(true);
            Object.SetTransfer(position, parallelToken);
            Object.SetSkin(skinId);
            Object.SetTeamProperty(team);
            Object.SetAgentAim(aim);
            if (power > 0)
            {
                Object.AddBooster(BoosterId.Power, power, true);
            }

            if (Object.Character.Info?.Type == CharacterType.Effect)
            {
                Object.SetAnchorProperty(position);
                Object.SetBirthIdProperty(birthId);
            }

            TotalSecondsSet = _time.TotalSeconds;
        }

        private void Object_UpdateBefore(object? _, ParallelToken e)
        {
            if (!Object.Alive.Alive.CurrentValue
                && (!Object.Alive.Cycles.CanUpdate || Object.Alive.Cycles.Value.CurrentValue.B >= 1))
            {
                Object.ResetOrReserve(e);
            }
        }
    }
}
