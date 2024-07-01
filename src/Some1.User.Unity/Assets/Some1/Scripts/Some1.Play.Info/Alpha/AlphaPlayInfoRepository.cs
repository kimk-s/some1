namespace Some1.Play.Info.Alpha
{
    public sealed class AlphaPlayInfoRepository : MemoryPlayInfoRepository
    {
        public AlphaPlayInfoRepository(string environment) : this(AlphaPlayInfoEnvironmentParser.Parse(environment))
        {
        }

        public AlphaPlayInfoRepository(AlphaPlayInfoEnvironment environment) : base(AlphaPlayInfoFactory.Create(environment))
        {
        }
    }
}
