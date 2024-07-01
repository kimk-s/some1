using System;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerGameToastFront
    {
        public ReadOnlyReactiveProperty<PlayerGameToastState> State { get; }
    }
    
    public readonly struct PlayerGameToastState : IEquatable<PlayerGameToastState>
    {
        public PlayerGameToastState(PlayerGameToastStateType type, GameMode? gameMode, int time, int score, bool success)
        {
            Type = type;
            GameMode = gameMode;
            Time = time;
            Score = score;
            Success = success;
        }

        public PlayerGameToastStateType Type { get; }

        public GameMode? GameMode { get; }

        public int Time { get; }

        public int Score { get; }

        public bool Success { get; }

        public override bool Equals(object? obj) => obj is PlayerGameToastState other && Equals(other);

        public bool Equals(PlayerGameToastState other) => Type == other.Type && GameMode == other.GameMode && Time == other.Time && Score == other.Score && Success == other.Success;

        public override int GetHashCode() => HashCode.Combine(Type, GameMode, Time, Score, Success);

        public override string ToString() => $"<{Type} {GameMode} {Time} {Score} {Success}>";

        public static bool operator ==(PlayerGameToastState left, PlayerGameToastState right) => left.Equals(right);

        public static bool operator !=(PlayerGameToastState left, PlayerGameToastState right) => !(left == right);
    }

    public enum PlayerGameToastStateType
    {
        None,
        Ready,
        Return,
        ReReady,
        ReturnFaulted,
        Start,
        End,
    }
}
