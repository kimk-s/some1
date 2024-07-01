namespace Some1.Play.Info
{
    public readonly struct CastAimInfo
    {
        public CastAimInfo(
            CastAimType type,
            AreaInfo area,
            float length,
            float offset,
            float angle,
            ObjectTargetInfo target,
            BumpLevel? bumpLevel)
        {
            Type = type;
            Area = area;
            Length = length;
            Angle = angle;
            Offset = offset;
            Target = target;
            BumpLevel = bumpLevel;
        }

        public CastAimType Type { get; }

        public AreaInfo Area { get; }

        public float Length { get; }

        public float Offset { get; }

        public float Angle { get; }

        public ObjectTargetInfo Target { get; }

        public BumpLevel? BumpLevel { get; }
    }
}
