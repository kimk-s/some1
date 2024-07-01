using System;

namespace Some1.Play.Info
{
    public readonly struct TeamTarget : IEquatable<TeamTarget>
    {
        public TeamTarget(TeamTargetInfo info, byte team)
        {
            Team = team;
            Info = info;
        }

        public static TeamTarget All => new(TeamTargetInfo.All, Play.Info.Team.Neutral);

        public static TeamTarget None => new(TeamTargetInfo.None, Play.Info.Team.Neutral);

        public TeamTargetInfo Info { get; }

        public byte Team { get; }

        public bool IsMatch(byte otherTeam)
        {
            if (Info == TeamTargetInfo.All)
            {
                return true;
            }
            
            if (Info.HasFlag(TeamTargetInfo.Neutral)
                && otherTeam == Play.Info.Team.Neutral)
            {
                return true;
            }

            if (Info.HasFlag(TeamTargetInfo.Ally)
                && otherTeam != Play.Info.Team.Neutral
                && otherTeam == Team)
            {
                return true;
            }

            if (Info.HasFlag(TeamTargetInfo.Enemy)
                && otherTeam != Play.Info.Team.Neutral
                && Team != Play.Info.Team.Neutral
                && otherTeam != Team)
            {
                return otherTeam != Team;
            }
            
            return false;
        }

        public override bool Equals(object? obj) => obj is TeamTarget other && Equals(other);

        public bool Equals(TeamTarget other) => Info == other.Info && Team == other.Team;

        public override int GetHashCode() => HashCode.Combine(Info, Team);

        public override string ToString() => $"<{Info} {Team}>";

        public static bool operator ==(TeamTarget left, TeamTarget right) => left.Equals(right);

        public static bool operator !=(TeamTarget left, TeamTarget right) => !(left == right);
    }
}
