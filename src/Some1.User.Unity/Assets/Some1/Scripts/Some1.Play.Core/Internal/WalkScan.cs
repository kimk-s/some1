using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal readonly struct WalkScan
    {
        internal WalkScan(Area source, float length, ObjectTarget target, BumpLevel bumpLevel)
        {
            Source = source;
            Length = length;
            Target = target;
            BumpLevel = bumpLevel;
        }

        internal Area Source { get; }

        internal float Length { get; }

        internal ObjectTarget Target { get; }

        internal BumpLevel BumpLevel { get; }

        public override string ToString() => $"<{Source} {Length} {Target} {BumpLevel}>";

        internal Area GetArea() => Area.Circle(Source.Position, Length * 2 + Source.Size.Width);
    }
}
