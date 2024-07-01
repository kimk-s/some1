using System.Drawing;

namespace Some1.Play.Info
{
    public sealed class AreaInfo
    {
        public AreaInfo(AreaType type, Aim position, float size)
            : this(type, position, new SizeF(size, size))
        {
        }

        public AreaInfo(AreaType type, Aim position, SizeF size)
        {
            Type = type;
            Size = size;
            Position = position;
        }

        public static AreaInfo Empty { get; } = new(default, default, 0);

        public AreaType Type { get; }

        public Aim Position { get; }

        public SizeF Size { get; }
    }
}
