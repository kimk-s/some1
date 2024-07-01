using System;
using System.Diagnostics;

namespace Some1.Play.Info
{
    public enum LevelingMethod
    {
        Plain,
        Accumulated,
        Pow
    }

    public readonly struct Leveling : IEquatable<Leveling>
    {
        public Leveling(int point, int maxLevel, int stepFactor, LevelingMethod method)
        {
            if (point < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(point));
            }

            if (maxLevel < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxLevel));
            }

            if (stepFactor < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stepFactor));
            }

            int level = 0;
            int stepMinPoint;
            int stepMaxPoint = 0;
            while (true)
            {
                stepMinPoint = stepMaxPoint;

                if (level >= maxLevel)
                {
                    break;
                }

                stepMaxPoint = method switch
                {
                    LevelingMethod.Plain => stepFactor * (level + 1),
                    LevelingMethod.Accumulated => stepMinPoint + stepFactor * (level + 1),
                    LevelingMethod.Pow => (int)MathF.Pow(stepFactor, level),
                    _ => throw new InvalidOperationException()
                };

                if (point < stepMaxPoint)
                {
                    break;
                }

                level++;
            }

            Level = level;
            Point = point;
            MaxLevel = maxLevel;
            StepMinPoint = stepMinPoint;
            StepMaxPoint = stepMaxPoint;

            Debug.Assert(Level <= MaxLevel);
        }

        public int Level { get; }

        public int Point { get; }

        public int MaxLevel { get; }

        public int StepMinPoint { get; }

        public int StepMaxPoint { get; }

        public float StepPointRate => StepPointLength == 0 ? 1 : (float)StepPoint / StepPointLength;

        public bool IsMaxLevel => Level == MaxLevel;

        private int StepPointLength => StepMaxPoint - StepMinPoint;

        private int StepPoint => Point - StepMinPoint;

        public override bool Equals(object? obj) => obj is Leveling other && Equals(other);

        public bool Equals(Leveling other) => Level == other.Level && Point == other.Point && MaxLevel == other.MaxLevel && StepMinPoint == other.StepMinPoint && StepMaxPoint == other.StepMaxPoint;

        public override int GetHashCode() => HashCode.Combine(Level, Point, MaxLevel, StepMinPoint, StepMaxPoint);

        public override string ToString() => $"<{Level} {Point} {MaxLevel} {StepMinPoint} {StepMaxPoint}>";

        public static bool operator ==(Leveling left, Leveling right) => left.Equals(right);

        public static bool operator !=(Leveling left, Leveling right) => !(left == right);
    }
}
