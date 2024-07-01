using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal readonly struct StrikeScan
    {
        internal StrikeScan(
            int sourceObjectId,
            Area sourceArea,
            float length,
            float offset,
            ObjectTarget target,
            BumpLevel? bumpLevel)
        {
            SourceObjectId = sourceObjectId;
            SourceArea = sourceArea;
            Length = length;
            Offset = offset;
            Target = target;
            BumpLevel = bumpLevel;
        }

        internal int SourceObjectId { get; }

        internal Area SourceArea { get; }

        internal float Length { get; }

        internal float Offset { get; }

        internal ObjectTarget Target { get; }

        internal BumpLevel? BumpLevel { get; }

        public override string ToString() => $"<{SourceObjectId} {SourceArea} {Length} {Offset} {Target} {BumpLevel}>";

        internal Area GetArea() => Area.Circle(SourceArea.Position, Length * 2 + SourceArea.Size.Width);
    }
}
