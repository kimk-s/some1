using System;
using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal struct Strike
    {
        private Vector2? _leftDelta;
        private Vector2? _rightDelta;
        private Vector2? _randomDelta;

        public Strike(Area source, Area destination, BumpLevel? bumpLevel)
        {
            Source = source;
            Destination = destination;
            BumpLevel = bumpLevel;
            _leftDelta = null;
            _rightDelta = null;
            _randomDelta = null;
        }

        internal Area Source { get; }

        internal Area Destination { get; }

        internal BumpLevel? BumpLevel { get; }

        public override readonly string ToString() => $"<{Source} {Destination} {BumpLevel}>";

        internal Vector2 GetDelta(StrikeZone zone) => zone switch
        {
            StrikeZone.Center => Destination.Position - Source.Position,
            StrikeZone.Left => _leftDelta ??= GetDelta(90),
            StrikeZone.Right => _rightDelta ??= GetDelta(270),
            StrikeZone.Random => _randomDelta ??= GetDelta(90 + RandomForUnity.NextSingle() * 180),
            _ => throw new InvalidOperationException()
        };

        private Vector2 GetDelta(float sideRotation)
            => Destination.Position
            + new Aim(
                sideRotation + new Aim(GetDelta(StrikeZone.Center)).Rotation,
                Destination.Size.Width * 0.5f).ToVector2()
            - Source.Position;
    }
}
