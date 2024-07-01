using System;

namespace Some1.Play.Info
{
    public sealed class CharacterInfo
    {
        public CharacterInfo(
            CharacterId id,
            CharacterType type,
            CharacterAreaInfo area,
            BumpLevel? bumpLevel = null,
            CharacterRole? role = null,
            SeasonId? season = null,
            CharacterMoveInfo? move = null,
            CharacterShiftInfo? shift = null,
            CharacterWalkInfo? walk = null,
            HitReachedInfo? hitReached = null,
            EnergyReachedInfo? energyReached = null,
            ICharacterAgentInfo? agent = null,
            CharacterGiveStuffInfo? giveStuff = null)
        {
            if (area is null)
            {
                throw new ArgumentNullException(nameof(area));
            }

            if (bumpLevel is not null)
            {
                if (area.Size != 1 && area.Size != 2 && area.Size != 4)
                {
                    throw new InvalidOperationException("Invalid bump area size.");
                }

                if (move is not null || walk is not null)
                {
                    throw new InvalidOperationException("Invalid bump move or walk.");
                }
            }

            if ((type == CharacterType.Stuff && giveStuff is null) ||
                (type != CharacterType.Stuff && giveStuff is not null))
            {
                throw new ArgumentException("Invalid give stuff", nameof(giveStuff));
            }

            Id = id;
            Type = type;
            Area = area;
            BumpLevel = bumpLevel;
            Role = role;
            Season = season;
            Move = move;
            Shift = shift;
            Walk = walk;
            HitReached = hitReached;
            EnergyReached = energyReached;
            Agent = agent;
            GiveStuff = giveStuff;
        }

        public CharacterId Id { get; }

        public bool IsPlayer => Type == CharacterType.Player;

        public CharacterType Type { get; }

        public CharacterAreaInfo Area { get; }

        public BumpLevel? BumpLevel { get; }

        public CharacterRole? Role { get; }

        public SeasonId? Season { get; }

        public CharacterMoveInfo? Move { get; }

        public CharacterShiftInfo? Shift { get; }

        public CharacterWalkInfo? Walk { get; }

        public HitReachedInfo? HitReached { get; }

        public EnergyReachedInfo? EnergyReached { get; }

        public ICharacterAgentInfo? Agent { get; }

        public CharacterGiveStuffInfo? GiveStuff { get; }

        public bool IsBuffEnabled => Type.IsUnit();

        public bool IsBoosterEnabled => IsPlayer;

        public bool IsSpecialtyEnabled => IsPlayer;

        public bool IsTakeStuffEnabled => IsPlayer;

        public bool IsHitEnabled => Type.IsUnit();

        public bool IsLikeEnabled => IsPlayer;
    }
}
