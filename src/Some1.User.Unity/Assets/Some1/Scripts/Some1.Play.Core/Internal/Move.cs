using System.Numerics;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal readonly struct Move
    {
        internal Move(Area source, Vector2 delta, BumpLevel? bumpLevel, bool walk)
        {
            Source = source;
            Delta = delta;
            BumpLevel = bumpLevel;
            Walk = walk;
        }

        internal Area Source { get; }

        internal Area Destination => Source.AddPosition(Delta);

        internal Vector2 Delta { get; }

        internal BumpLevel? BumpLevel { get; }

        internal bool Walk { get; }

        public override string ToString() => $"<{Source} {Delta} {BumpLevel} {Walk}>";
    }
}
