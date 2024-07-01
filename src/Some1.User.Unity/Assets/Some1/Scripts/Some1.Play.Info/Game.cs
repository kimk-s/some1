using System;

namespace Some1.Play.Info
{
    public readonly struct Game : IEquatable<Game>
    {
        public Game(GameMode mod, int score)
        {
            Mode = mod;
            Score = score;
        }

        public GameMode Mode { get; }

        public int Score { get; }

        public Game WithScore(int score) => new(Mode, score);

        public override bool Equals(object? obj) => obj is Game other && Equals(other);

        public bool Equals(Game other) => Mode == other.Mode && Score == other.Score;

        public override int GetHashCode() => HashCode.Combine(Mode, Score);

        public override string ToString() => $"<{Mode} {Score}>";

        public static bool operator ==(Game left, Game right) => left.Equals(right);

        public static bool operator !=(Game left, Game right) => !(left == right);
    }
}
