using System;
using System.Numerics;

namespace Some1
{
    public interface IWave<TSelf, TValue> : IEquatable<TSelf>
        where TValue : unmanaged, IEquatable<TValue>
    {
        TValue A { get; }
        TValue B { get; }
        TValue Delta { get; }
        TSelf Flow(TValue delta);
        TSelf Hold(TValue value);
        TValue Interpolate(float time);
        bool CanExtrapolateByFps(FloatWave fps, TSelf from, TValue tolerance);
        TSelf ExtrapolateByDeltaTime(FloatWave deltaTime);
    }

    public readonly struct FloatWave : IWave<FloatWave, float>
    {
        public FloatWave(float ab) : this(ab, ab) { }
        public FloatWave(float a, float b) => (A, B) = (a, b);

        public static FloatWave Zero { get; } = new();
        public float A { get; }
        public float B { get; }
        public float Delta => B - A;

        public FloatWave Flow(float delta) => new(B, B + delta);

        public FloatWave Hold(float value) => new(value);

        public float Interpolate(float time) => A + Delta * time;

        public bool CanExtrapolateByFps(FloatWave fps, FloatWave from, float tolerance)
            => from.B.Equals(A) && EqualsWithTolerance(from.GetVelocityByFPS(fps.A), GetVelocityByFPS(fps.B), tolerance);
            //=> from.B.Equals(A) && EqualsWithTolerance(from.Delta * deltaTime.B, Delta * deltaTime.A, tolerance);

        public FloatWave ExtrapolateByDeltaTime(FloatWave deltaTime) => Flow(GetVelocityByDeltaTime(deltaTime.A) * deltaTime.B);

        private float GetVelocityByFPS(float fps) => Delta * fps;

        private float GetVelocityByDeltaTime(float deltaTime) => Delta / deltaTime;

        private static bool EqualsWithTolerance(float a, float b, float tolerance) => MathF.Abs(a - b) <= tolerance;

        public FloatWave Inverse() => new(A == 0 ? 0 : (1 / A), B == 0 ? 0 : (1 / B));

        public override bool Equals(object? obj) => obj is FloatWave other && Equals(other);
        public bool Equals(FloatWave other) => A == other.A && B == other.B;
        public override int GetHashCode() => HashCode.Combine(A, B);
        public override string ToString() => $"<{A} {B}>";
        public static explicit operator FloatWave(float ab) => new(ab);
        public static bool operator ==(FloatWave left, FloatWave right) => left.Equals(right);
        public static bool operator !=(FloatWave left, FloatWave right) => !(left == right);
    }

    public readonly struct DoubleWave : IWave<DoubleWave, double>
    {
        public DoubleWave(double ab) : this(ab, ab) { }
        public DoubleWave(double a, double b) => (A, B) = (a, b);

        public static DoubleWave Zero { get; } = new();
        public double A { get; }
        public double B { get; }
        public double Delta => B - A;

        public DoubleWave Flow(double delta) => new(B, B + delta);

        public DoubleWave Hold(double value) => new(value);

        public double Interpolate(float time) => A + Delta * time;

        public bool CanExtrapolateByFps(FloatWave fps, DoubleWave from, double tolerance)
            => from.B.Equals(A) && EqualsWithTolerance(from.GetVelocityByFPS(fps.A), GetVelocityByFPS(fps.B), tolerance);
            //=> from.B.Equals(A) && EqualsWithTolerance(from.Delta * deltaTime.B, Delta * deltaTime.A, tolerance);

        public DoubleWave ExtrapolateByDeltaTime(FloatWave deltaTime) => Flow(GetVelocityByDeltaTime(deltaTime.A) * deltaTime.B);

        private double GetVelocityByFPS(float fps) => Delta * fps;

        private double GetVelocityByDeltaTime(float deltaTime) => Delta / deltaTime;

        private static bool EqualsWithTolerance(double a, double b, double tolerance) => Math.Abs(a - b) <= tolerance;

        public override bool Equals(object? obj) => obj is DoubleWave other && Equals(other);
        public bool Equals(DoubleWave other) => A == other.A && B == other.B;
        public override int GetHashCode() => HashCode.Combine(A, B);
        public override string ToString() => $"<{A} {B}>";
        public static explicit operator DoubleWave(double ab) => new(ab);
        public static bool operator ==(DoubleWave left, DoubleWave right) => left.Equals(right);
        public static bool operator !=(DoubleWave left, DoubleWave right) => !(left == right);
    }

    public readonly struct Vector2Wave : IWave<Vector2Wave, Vector2>
    {
        public Vector2Wave(Vector2 ab) : this(ab, ab) { }
        public Vector2Wave(Vector2 a, Vector2 b) => (A, B) = (a, b);

        public static Vector2Wave Zero { get; } = new();
        public Vector2 A { get; }
        public Vector2 B { get; }
        public Vector2 Delta => B - A;

        public Vector2Wave Flow(Vector2 delta) => new(B, B + delta);

        public Vector2Wave Hold(Vector2 value) => new(value);

        public Vector2 Interpolate(float time) => A + Delta * time;

        public bool CanExtrapolateByFps(FloatWave fps, Vector2Wave from, Vector2 tolerance)
            => from.B.Equals(A) && EqualsWithTolerance(from.GetVelocityByFPS(fps.A), GetVelocityByFPS(fps.B), tolerance);
            //=> from.B.Equals(A) && EqualsWithTolerance(from.Delta * deltaTime.B, Delta * deltaTime.A, tolerance);

        public Vector2Wave ExtrapolateByDeltaTime(FloatWave deltaTime) => Flow(GetVelocityByDeltaTime(deltaTime.A) * deltaTime.B);

        private Vector2 GetVelocityByFPS(float fps) => Delta * fps;

        private Vector2 GetVelocityByDeltaTime(float deltaTime) => Delta / deltaTime;

        private static bool EqualsWithTolerance(Vector2 a, Vector2 b, Vector2 tolerance) => MathF.Abs(a.X - b.X) <= tolerance.X && MathF.Abs(a.Y - b.Y) <= tolerance.Y;

        public override bool Equals(object? obj) => obj is Vector2Wave other && Equals(other);
        public bool Equals(Vector2Wave other) => A.Equals(other.A) && B.Equals(other.B);
        public override int GetHashCode() => HashCode.Combine(A, B, Delta);
        public override string ToString() => $"<{A} {B}>";
        public static explicit operator Vector2Wave(Vector2 ab) => new(ab);
        public static bool operator ==(Vector2Wave left, Vector2Wave right) => left.Equals(right);
        public static bool operator !=(Vector2Wave left, Vector2Wave right) => !(left == right);
    }
}
