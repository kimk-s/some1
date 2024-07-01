namespace Some1.User.Unity
{
    public class ProgramArgs
    {
        public ProgramArgs(ProgramEnvironment environment, ProgramConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public ProgramEnvironment Environment { get; }
        public ProgramConfiguration Configuration { get; }
    }
}
