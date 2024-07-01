using System.Numerics;

namespace Some1.Play.Info
{
    public static class AreaExtensions
    {
        public static Area AddPosition(this Area area, Vector2 delta)
        {
            return new(
                area.Type,
                new(
                    area.Data.Location.X + delta.X,
                    area.Data.Location.Y + delta.Y,
                    area.Data.Size.Width,
                    area.Data.Size.Height));
        }

        public static Vector2 GetRandomPosition(this Area area)
        {
            return area.Location.ToVector2() + area.Size.ToVector2() * new Vector2(RandomForUnity.NextSingle(), RandomForUnity.NextSingle());
        }
    }
}
